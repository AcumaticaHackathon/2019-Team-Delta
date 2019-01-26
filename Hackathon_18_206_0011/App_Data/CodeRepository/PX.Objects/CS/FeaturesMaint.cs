﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PX.Common;
using PX.Data;
using PX.Objects.GL.DAC;
using PX.Objects.GL.FinPeriods;
using PX.Objects.WZ;
using PX.Objects.AR;
using PX.Objects.SO;

namespace PX.Objects.CS
{

	public class FeaturesMaint : PXGraph<FeaturesMaint>
	{
        public PXFilter<AfterActivation> ActivationBehaviour;
        
		public PXSelect<FeaturesSet> Features;

		protected IEnumerable features()
		{
			FeaturesSet current = (FeaturesSet)PXSelect<FeaturesSet,
				                  Where<True, Equal<True>>,
				                  OrderBy<Desc<FeaturesSet.status>>>
				                  .SelectWindowed(this, 0, 1) ?? Features.Insert();
			current.LicenseID = PXVersionInfo.InstallationID;
			yield return current;				
		}

        public FeaturesMaint()
	    {
	        SaveClose.SetVisible(false);
	    }

		public PXSave<FeaturesSet> Save;
	    public PXSaveClose<FeaturesSet> SaveClose;
		public PXCancel<FeaturesSet> Cancel;
		public PXAction<FeaturesSet> Insert;

		public PXAction<FeaturesSet> RequestValidation;
		public PXAction<FeaturesSet> CancelRequest;

		public PXSelectJoin<
						MasterFinPeriod,
						InnerJoin<OrganizationFinPeriod,
							On<MasterFinPeriod.finPeriodID, Equal<OrganizationFinPeriod.masterFinPeriodID>,
							And<OrganizationFinPeriod.organizationID, Equal<Required<OrganizationFinPeriod.organizationID>>>>>> MasterFinPeriods;

		public const int MAX_FINPERIOD_DISCREPANCY_MESSAGE_COUNT = 20;

		public override IEnumerable ExecuteSelect(string viewName, object[] parameters, object[] searches, string[] sortcolumns, bool[] descendings, PXFilterRow[] filters, ref int startRow, int maximumRows, ref int totalRows)
		{
			if (viewName == "Features")
				searches = null;

			return base.ExecuteSelect(viewName, parameters, searches, sortcolumns, descendings, filters, ref startRow, maximumRows, ref totalRows);
		}

		[PXButton]
		[PXUIField(DisplayName = "Modify", MapEnableRights = PXCacheRights.Insert, MapViewRights = PXCacheRights.Select)]
		public IEnumerable insert(PXAdapter adapter)
		{
		    var activationMode = this.ActivationBehaviour.Current;
		    foreach (var item in new PXInsert<FeaturesSet>(this, "Insert").Press(adapter))
            {
                this.ActivationBehaviour.Cache.SetValueExt<AfterActivation.refresh>(this.ActivationBehaviour.Current, activationMode.Refresh);
		        yield return item;
		    }
		    
		}

		[PXButton]
		[PXUIField(DisplayName = "Enable")]
		public IEnumerable requestValidation(PXAdapter adapter)
		{
			foreach (FeaturesSet feature in adapter.Get())
			{
				if (feature.Status == 3)
				{
					bool? customerDiscountsOld = PXAccess.FeatureInstalled<FeaturesSet.customerDiscounts>();
                    PXCache cache = new PXCache<FeaturesSet>(this);			
					FeaturesSet update = PXCache<FeaturesSet>.CreateCopy(feature);
					update.Status = 0;					
					update = this.Features.Update(update);
					this.Features.Delete(feature);
					
					if (update.Status != 1)					
						this.Features.Delete(new FeaturesSet() {Status = 1});
					

					this.Persist();
                    
                    var tasks = PXSelect<WZTask>.Select(this);
                    WZTaskEntry taskGraph = CreateInstance<WZTaskEntry>();
                    foreach (WZTask task in tasks)
                    {
                        bool disableTask = false;
                        bool enableTask = false;
                        foreach (
                            WZTaskFeature taskFeature in
                                PXSelectReadonly<WZTaskFeature, Where<WZTaskFeature.taskID, Equal<Required<WZTask.taskID>>>>.Select(
                                    this, task.TaskID))
                        {
                            bool featureInstalled = (bool?)cache.GetValue(update, taskFeature.Feature) == true;

                            if (!featureInstalled)
                            {
                                disableTask = true;
                                enableTask = false;
                                break;
                            }

                            enableTask = true;
                        }

                        if (disableTask)
                        {
                            task.Status = WizardTaskStatusesAttribute._DISABLED;
                            taskGraph.TaskInfo.Update(task);
                            taskGraph.Save.Press();
                        }

                        if (enableTask && task.Status == WizardTaskStatusesAttribute._DISABLED)
                        {

                            bool needToBeOpen = false;
                            WZScenario scenario = PXSelect<WZScenario, Where<WZScenario.scenarioID, Equal<Required<WZTask.scenarioID>>>>.Select(this, task.ScenarioID);
                            if (scenario != null && scenario.Status == WizardScenarioStatusesAttribute._ACTIVE)
                            {
                                WZTask parentTask =
                                    PXSelect<WZTask, Where<WZTask.taskID, Equal<Required<WZTask.parentTaskID>>>>.Select(
                                        this, task.ParentTaskID);

                                if (parentTask != null && (parentTask.Status == WizardTaskStatusesAttribute._OPEN ||
                                                           parentTask.Status == WizardTaskStatusesAttribute._ACTIVE))
                                {
                                    needToBeOpen = true;
                                }

                                foreach (
                                    PXResult<WZTaskPredecessorRelation, WZTask> predecessorResult in
                                        PXSelectJoin<WZTaskPredecessorRelation,
                                            InnerJoin
                                                <WZTask,
                                                    On<WZTask.taskID, Equal<WZTaskPredecessorRelation.predecessorID>>>,
                                            Where<WZTaskPredecessorRelation.taskID, Equal<Required<WZTask.taskID>>>>.
                                            Select(this, task.TaskID))
                                {
                                    WZTask predecessorTask = (WZTask) predecessorResult;
                                    if (predecessorTask != null)
                                    {
                                        if (predecessorTask.Status == WizardTaskStatusesAttribute._COMPLETED)
                                        {
                                            needToBeOpen = true;

                                        }
                                        else
                                        {
                                            needToBeOpen = false;
                                            break;
                                        }
                                    }
                                }
                            }
                            task.Status = needToBeOpen ? WizardTaskStatusesAttribute._OPEN : WizardTaskStatusesAttribute._PENDING;
                            taskGraph.TaskInfo.Update(task);
                            taskGraph.Save.Press();
                        }
                    }

					if (customerDiscountsOld == true && update.CustomerDiscounts != true)
					{
						PXUpdate<Set<ARSetup.applyLineDiscountsIfCustomerPriceDefined, True>, ARSetup>.Update(this);
						PXUpdate<Set<ARSetup.applyLineDiscountsIfCustomerClassPriceDefined, True>, ARSetup>.Update(this);
						PXUpdate<Set<SOOrderType.recalculateDiscOnPartialShipment, False, Set<SOOrderType.postLineDiscSeparately, False>>, SOOrderType>.Update(this);
					}

					yield return update;
				}
				else
					yield return feature;
			}

		    bool needRefresh = !(ActivationBehaviour.Current != null && ActivationBehaviour.Current.Refresh == false);

		    PXDatabase.ResetSlots();
			PXPageCacheUtils.InvalidateCachedPages();
			this.Clear();
			if (needRefresh)
				throw new PXRefreshException();
		}

		[PXButton]
		[PXUIField(DisplayName = "Cancel Validation Request", Visible = false)]
		public IEnumerable cancelRequest(PXAdapter adapter)
		{
			foreach (FeaturesSet feature in adapter.Get())
			{
				if (feature.Status == 2)
				{
					FeaturesSet update = PXCache<FeaturesSet>.CreateCopy(feature);
					update.Status = 3;					
					this.Features.Delete(feature);
					update = this.Features.Update(update);
					this.Persist();
					yield return update;
				}
				else
					yield return feature;
			}
		}

		protected virtual void FeaturesSet_RowSelected(PXCache sender, PXRowSelectedEventArgs e)
		{
            this.Save.SetVisible(false);
			this.Features.Cache.AllowInsert = true;
			FeaturesSet row = (FeaturesSet)e.Row;
			if (row == null) return;

			this.RequestValidation.SetEnabled(row.Status == 3);
			this.CancelRequest.SetEnabled(row.Status == 2);
			this.Features.Cache.AllowInsert = row.Status < 2;
			this.Features.Cache.AllowUpdate = row.Status == 3;
			this.Features.Cache.AllowDelete = false;

            bool screenIsOpenedFromScenario =!(ActivationBehaviour.Current != null && ActivationBehaviour.Current.Refresh == true);
            if (screenIsOpenedFromScenario && this.Actions.Contains("CancelClose"))
                    this.Actions["CancelClose"].SetTooltip(WZ.Messages.BackToScenario);
		}

		protected virtual void FeaturesSet_RowInserting(PXCache sender, PXRowInsertingEventArgs e)
		{
			int? status = (int?)sender.GetValue<FeaturesSet.status>(e.Row);
			if (status != 3) return;

			FeaturesSet current = PXSelect<FeaturesSet,
				Where<True, Equal<True>>,
				OrderBy<Desc<FeaturesSet.status>>>
				.SelectWindowed(this,0,1);
			if (current != null)
			{
				sender.RestoreCopy(e.Row, current);
				sender.SetValue<FeaturesSet.status>(e.Row, 3);
			}
		}

		protected virtual void _(Events.FieldUpdating<FeaturesSet.centralizedPeriodsManagement> e)
		{
			e.NewValue = PXBoolAttribute.ConvertValue(e.NewValue);
			FeaturesSet row = (FeaturesSet)e.Row;
			if (row == null) return;

			int messageCount = 0;
			bool isError = false;

			if (row.CentralizedPeriodsManagement != null && row.CentralizedPeriodsManagement != (bool)e.NewValue && (bool)e.NewValue == true)
			{
				foreach (Organization organization in PXSelect<Organization>.Select(this))
				{
					foreach (MasterFinPeriod problemPeriod in PXSelectJoin<
						MasterFinPeriod,
						LeftJoin<OrganizationFinPeriod,
							On<MasterFinPeriod.finPeriodID, Equal<OrganizationFinPeriod.masterFinPeriodID>,
							And<OrganizationFinPeriod.organizationID, Equal<Required<OrganizationFinPeriod.organizationID>>>>>,
						Where<OrganizationFinPeriod.finPeriodID, IsNull>>
						.Select(this, organization.OrganizationID))
					{
						isError = true;
						if (messageCount <= MAX_FINPERIOD_DISCREPANCY_MESSAGE_COUNT)
						{
							PXTrace.WriteError(GL.Messages.DiscrepancyPeriod, organization.OrganizationCD, problemPeriod.FinPeriodID);

							messageCount++;
						}
						else
						{
							break;
						}
					}
				}

				if (isError)
				{
					e.Cancel = true;
					throw new PXSetPropertyException(GL.Messages.DiscrepancyPeriodError);
				}

				Organization etalonOrganization = PXSelect<Organization>.Select(this).First();
				if (etalonOrganization != null)
				{
					foreach (Organization organization in PXSelect<Organization>.Select(this))
					{
						if (organization.OrganizationID == etalonOrganization.OrganizationID)
							continue;

						foreach (OrganizationFinPeriod problemPeriod in PXSelectJoin<
							OrganizationFinPeriod,
							LeftJoin<OrganizationFinPeriodStatus,
								On<OrganizationFinPeriodStatus.organizationID, Equal<Required<OrganizationFinPeriodStatus.organizationID>>,
								And<OrganizationFinPeriod.finPeriodID, Equal<OrganizationFinPeriodStatus.finPeriodID>,
								And<OrganizationFinPeriod.dateLocked, Equal<OrganizationFinPeriodStatus.dateLocked>,
								And<OrganizationFinPeriod.status, Equal<OrganizationFinPeriodStatus.status>,
								And<OrganizationFinPeriod.aPClosed, Equal<OrganizationFinPeriodStatus.aPClosed>,
								And<OrganizationFinPeriod.aRClosed, Equal<OrganizationFinPeriodStatus.aRClosed>,
								And<OrganizationFinPeriod.iNClosed, Equal<OrganizationFinPeriodStatus.iNClosed>,
								And<OrganizationFinPeriod.cAClosed, Equal<OrganizationFinPeriodStatus.cAClosed>,
								And<OrganizationFinPeriod.fAClosed, Equal<OrganizationFinPeriodStatus.fAClosed>>>>>>>>>>>,
							Where<OrganizationFinPeriodStatus.finPeriodID, IsNull,
								And<OrganizationFinPeriod.organizationID, Equal<Required<OrganizationFinPeriod.organizationID>>>>>
						.Select(this, organization.OrganizationID, etalonOrganization.OrganizationID))
						{
							isError = true;
							if (messageCount <= MAX_FINPERIOD_DISCREPANCY_MESSAGE_COUNT)
							{
								string problemFields = GetProblemFields(organization, problemPeriod);

								PXTrace.WriteError(GL.Messages.DiscrepancyField,
									etalonOrganization.OrganizationCD,
									organization.OrganizationCD,
									problemFields,
									problemPeriod.FinPeriodID);

								messageCount++;
							}
							else
							{
								break;
							}
						}
					}

					if (isError)
					{
						e.Cancel = true;
						throw new PXSetPropertyException(GL.Messages.DiscrepancyFieldError);
					}
				}

				foreach (PXResult<MasterFinPeriod, OrganizationFinPeriod> res in MasterFinPeriods.Select(etalonOrganization.OrganizationID))
				{
					MasterFinPeriod masterFinPeriod = res;
					OrganizationFinPeriod organizationFinPeriod = res;

					masterFinPeriod.DateLocked = organizationFinPeriod.DateLocked;
					masterFinPeriod.Status = organizationFinPeriod.Status;
					masterFinPeriod.APClosed = organizationFinPeriod.APClosed;
					masterFinPeriod.ARClosed = organizationFinPeriod.ARClosed;
					masterFinPeriod.INClosed = organizationFinPeriod.INClosed;
					masterFinPeriod.CAClosed = organizationFinPeriod.CAClosed;
					masterFinPeriod.FAClosed = organizationFinPeriod.FAClosed;

					this.MasterFinPeriods.Cache.Update(masterFinPeriod);
				}
			}
		}

		private string GetProblemFields(Organization organization, OrganizationFinPeriod problemPeriod)
		{
			OrganizationFinPeriod currentFinPeriod = PXSelect<
				OrganizationFinPeriod,
				Where<OrganizationFinPeriod.organizationID, Equal<Required<OrganizationFinPeriod.organizationID>>,
					And<OrganizationFinPeriod.finPeriodID, Equal<Required<OrganizationFinPeriod.finPeriodID>>>>>
				.Select(this, organization.OrganizationID, problemPeriod.FinPeriodID);

			List<string> fieldList = new List<string>();
			if (problemPeriod.DateLocked != currentFinPeriod.DateLocked)
				fieldList.Add(nameof(problemPeriod.DateLocked));

			if (problemPeriod.Status != currentFinPeriod.Status)
				fieldList.Add(nameof(problemPeriod.Status));

			if (problemPeriod.APClosed != currentFinPeriod.APClosed)
				fieldList.Add(nameof(problemPeriod.APClosed));

			if (problemPeriod.ARClosed != currentFinPeriod.ARClosed)
				fieldList.Add(nameof(problemPeriod.ARClosed));

			if (problemPeriod.INClosed != currentFinPeriod.INClosed)
				fieldList.Add(nameof(problemPeriod.INClosed));

			if (problemPeriod.CAClosed != currentFinPeriod.CAClosed)
				fieldList.Add(nameof(problemPeriod.CAClosed));

			if (problemPeriod.FAClosed != currentFinPeriod.FAClosed)
				fieldList.Add(nameof(problemPeriod.FAClosed));

			return String.Join(", ", fieldList.ToArray());
		}
	}

    [Serializable]
    public partial class AfterActivation : IBqlTable
    {
        #region Refresh
        public abstract class refresh : IBqlField { }

        protected Boolean? _Refresh;
        [PXDBBool]
        public virtual Boolean? Refresh
        {
            get
            {
                return this._Refresh;
            }
            set
            {
                this._Refresh = value;
            }
        }
        #endregion
    }
}