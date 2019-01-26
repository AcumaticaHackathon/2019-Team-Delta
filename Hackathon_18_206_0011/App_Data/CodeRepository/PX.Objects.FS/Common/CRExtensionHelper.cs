﻿using PX.Data;
using PX.Objects.CR;
using PX.Objects.EP;
using PX.Objects.IN;
using System;
using System.Collections.Generic;

namespace PX.Objects.FS
{
    public static class CRExtensionHelper
    {
        public static void LaunchEmployeeBoard(PXGraph graph, int? soID)
        {
            if (soID == null)
            {
                return;
            }

            FSServiceOrder fsServiceOrderRow = GetServiceOrder(graph, soID);

            if (fsServiceOrderRow != null)
            {
                ServiceOrderEntry graphServiceOrder = PXGraph.CreateInstance<ServiceOrderEntry>();
                graphServiceOrder.ServiceOrderRecords.Current = graphServiceOrder.ServiceOrderRecords
                                .Search<FSServiceOrder.refNbr>(fsServiceOrderRow.RefNbr, fsServiceOrderRow.SrvOrdType);
                graphServiceOrder.OpenEmployeeBoard();
            }
        }

        public static void LaunchServiceOrderScreen(PXGraph graph, int? soID)
        {
            if (soID == null)
            {
                return;
            }

            FSServiceOrder fsServiceOrderRow = GetServiceOrder(graph, soID);

            if (fsServiceOrderRow != null)
            {
                ServiceOrderEntry graphServiceOrder = PXGraph.CreateInstance<ServiceOrderEntry>();
                graphServiceOrder.ServiceOrderRecords.Current = graphServiceOrder.ServiceOrderRecords
                                .Search<FSServiceOrder.refNbr>(fsServiceOrderRow.RefNbr, fsServiceOrderRow.SrvOrdType);
                throw new PXRedirectRequiredException(graphServiceOrder, null) { Mode = PXBaseRedirectException.WindowMode.NewWindow};
            }
        }

        public static FSServiceOrder GetServiceOrder(PXGraph graph, int? soID)
        {
            return (FSServiceOrder)
                PXSelect<FSServiceOrder,
                Where<
                    FSServiceOrder.sOID, Equal<Required<FSServiceOrder.sOID>>>>
                .Select(graph, soID);
        }

        private static bool IsTheRequiredFieldsPresent(PXGraph graph, CROpportunity crOpportunityRow, FSxCROpportunity fsxCROpportunityRow)
        {
            if (crOpportunityRow == null
                || fsxCROpportunityRow == null 
                || crOpportunityRow.ClassID == null
                || crOpportunityRow.BranchID == null
                || crOpportunityRow.Subject == null
                || crOpportunityRow.ProjectID == null
                || crOpportunityRow.CloseDate == null
                || fsxCROpportunityRow.SrvOrdType == null
                || fsxCROpportunityRow.BranchLocationID == null)
            {
                return false;
            }

            if (fsxCROpportunityRow.SrvOrdType != null)
            {
                FSSrvOrdType fsSrvOrdTypeRow = GetServiceOrderType(graph, fsxCROpportunityRow.SrvOrdType);

                if (fsSrvOrdTypeRow != null
                        && fsSrvOrdTypeRow.Behavior != ID.Behavior_SrvOrderType.INTERNAL_APPOINTMENT
                            && (crOpportunityRow.BAccountID == null || crOpportunityRow.LocationID == null))
                {
                    return false;
                }
            }

            return true;
        }

        private static bool IsTheRequiredFieldsPresent(PXGraph graph, CRCase crCaseRow, FSxCRCase fsxCRCaseRow)
        {
            return !(crCaseRow == null
                     || fsxCRCaseRow == null
                     || crCaseRow.ClassID == null
                     || crCaseRow.CustomerID == null
                     || crCaseRow.Subject == null
                     || fsxCRCaseRow.SrvOrdType == null
                     || fsxCRCaseRow.BranchLocationID == null);
        }

        private static void SaveServicerOrder(ServiceOrderEntry graphServiceOrderEntry)
        {
            if (graphServiceOrderEntry.IsDirty)
            {
                graphServiceOrderEntry.SelectTimeStamp();
                graphServiceOrderEntry.Save.Press();
            }
        }

        public static void UpdateServiceOrderHeader(
            ServiceOrderEntry graphServiceOrderEntry,
            PXCache cache,
            CRCase crCaseRow,
            FSCreateServiceOrderOnCaseFilter fsCreateServiceOrderOnCaseFilterRow,
            FSServiceOrder fsServiceOrderRow,
            bool updatingExistingSO)
        {
            if (fsServiceOrderRow.Status == ID.Status_ServiceOrder.CLOSED)
            {
                return;
            }

            bool somethingChanged = false;

            FSSrvOrdType fsSrvOrdTypeRow = GetServiceOrderType(graphServiceOrderEntry, fsServiceOrderRow.SrvOrdType);

            if (fsSrvOrdTypeRow.Behavior != ID.Behavior_SrvOrderType.INTERNAL_APPOINTMENT)
            {
                if (fsServiceOrderRow.CustomerID != crCaseRow.CustomerID)
                {
                    graphServiceOrderEntry.ServiceOrderRecords.SetValueExt<FSServiceOrder.customerID>(fsServiceOrderRow, crCaseRow.CustomerID);
                    somethingChanged = true;
                }

                if (fsServiceOrderRow.LocationID != crCaseRow.LocationID)
                {
                    graphServiceOrderEntry.ServiceOrderRecords.SetValueExt<FSServiceOrder.locationID>(fsServiceOrderRow, crCaseRow.LocationID);
                    somethingChanged = true;
                }
            }

            if (fsServiceOrderRow.BranchLocationID != fsCreateServiceOrderOnCaseFilterRow.BranchLocationID)
            {
                graphServiceOrderEntry.ServiceOrderRecords.SetValueExt<FSServiceOrder.branchLocationID>(fsServiceOrderRow, fsCreateServiceOrderOnCaseFilterRow.BranchLocationID);
                somethingChanged = true;
            }

            if (fsServiceOrderRow.ContactID != crCaseRow.ContactID)
            {
                graphServiceOrderEntry.ServiceOrderRecords.SetValueExt<FSServiceOrder.contactID>(fsServiceOrderRow, crCaseRow.ContactID);
                somethingChanged = true;
            }

            if (fsServiceOrderRow.DocDesc != crCaseRow.Subject)
            {
                graphServiceOrderEntry.ServiceOrderRecords.SetValueExt<FSServiceOrder.docDesc>(fsServiceOrderRow, crCaseRow.Subject);
                somethingChanged = true;
            }

            if (crCaseRow.OwnerID != null)
            {
                if (crCaseRow.OwnerID != (Guid?)cache.GetValueOriginal<CROpportunity.ownerID>(crCaseRow))
                {
                    int? salesPersonID = GetSalesPersonID(graphServiceOrderEntry, crCaseRow.OwnerID);

                    if (salesPersonID != null)
                    {
                        graphServiceOrderEntry.ServiceOrderRecords.SetValueExt<FSServiceOrder.salesPersonID>(fsServiceOrderRow, salesPersonID);
                        somethingChanged = true;
                    }
                }
            }

            if (crCaseRow.CreatedDateTime.HasValue 
                    && fsServiceOrderRow.OrderDate != crCaseRow.CreatedDateTime.Value.Date)
            {
                graphServiceOrderEntry.ServiceOrderRecords.SetValueExt<FSServiceOrder.orderDate>(fsServiceOrderRow, crCaseRow.CreatedDateTime.Value.Date);
                somethingChanged = true;
            }

            if (fsServiceOrderRow.SLAETA != crCaseRow.SLAETA)
            {
                graphServiceOrderEntry.ServiceOrderRecords.SetValueExt<FSServiceOrder.sLAETA>(fsServiceOrderRow, crCaseRow.SLAETA);
                somethingChanged = true;
            }

            if (fsServiceOrderRow.AssignedEmpID != fsCreateServiceOrderOnCaseFilterRow.AssignedEmpID)
            {
                graphServiceOrderEntry.ServiceOrderRecords.SetValueExt<FSServiceOrder.assignedEmpID>(fsServiceOrderRow, fsCreateServiceOrderOnCaseFilterRow.AssignedEmpID);
                somethingChanged = true;
            }

            if (fsServiceOrderRow.ProblemID != fsCreateServiceOrderOnCaseFilterRow.ProblemID)
            {
                graphServiceOrderEntry.ServiceOrderRecords.SetValueExt<FSServiceOrder.problemID>(fsServiceOrderRow, fsCreateServiceOrderOnCaseFilterRow.ProblemID);
                somethingChanged = true;
            }

            if (fsServiceOrderRow.LongDescr != crCaseRow.Description)
            {
                graphServiceOrderEntry.ServiceOrderRecords.SetValueExt<FSServiceOrder.longDescr>(fsServiceOrderRow, crCaseRow.Description);
                somethingChanged = true;
            }

            if (fsServiceOrderRow.Priority != crCaseRow.Priority)
            {
                graphServiceOrderEntry.ServiceOrderRecords.SetValueExt<FSServiceOrder.priority>(fsServiceOrderRow, crCaseRow.Priority);
                somethingChanged = true;
            }

            if (fsServiceOrderRow.PromisedDate != crCaseRow.ETA)
            {
                graphServiceOrderEntry.ServiceOrderRecords.SetValueExt<FSServiceOrder.promisedDate>(fsServiceOrderRow, crCaseRow.ETA);
                somethingChanged = true;
            }

            if (fsServiceOrderRow.Severity != crCaseRow.Severity)
            {
                graphServiceOrderEntry.ServiceOrderRecords.SetValueExt<FSServiceOrder.severity>(fsServiceOrderRow, crCaseRow.Severity);
                somethingChanged = true;
            }

            if (somethingChanged && updatingExistingSO)
            {
                graphServiceOrderEntry.ServiceOrderRecords.Update(fsServiceOrderRow);
            }
        }

        public static void UpdateServiceOrderHeader(
            ServiceOrderEntry graphServiceOrderEntry,
            PXCache cache,
            CROpportunity crOpportunityRow,
            FSCreateServiceOrderOnOpportunityFilter fsCreateServiceOrderOnOpportunityFilterRow,
            FSServiceOrder fsServiceOrderRow,
            CRContact crContactRow,
            CRAddress crAddressRow,
            bool updatingExistingSO)
        {
            bool somethingChanged = false;

            FSSrvOrdType fsSrvOrdTypeRow = GetServiceOrderType(graphServiceOrderEntry, fsServiceOrderRow.SrvOrdType);

            if (fsSrvOrdTypeRow.Behavior != ID.Behavior_SrvOrderType.INTERNAL_APPOINTMENT)
            {
                if (fsServiceOrderRow.CustomerID != crOpportunityRow.BAccountID)
                {
                    graphServiceOrderEntry.ServiceOrderRecords.SetValueExt<FSServiceOrder.customerID>(fsServiceOrderRow, crOpportunityRow.BAccountID);
                    somethingChanged = true;
                }

                if (fsServiceOrderRow.LocationID != crOpportunityRow.LocationID)
                {
                    graphServiceOrderEntry.ServiceOrderRecords.SetValueExt<FSServiceOrder.locationID>(fsServiceOrderRow, crOpportunityRow.LocationID);
                    somethingChanged = true;
                }
            }

            if (fsServiceOrderRow.BranchID != crOpportunityRow.BranchID)
            {
                graphServiceOrderEntry.ServiceOrderRecords.SetValueExt<FSServiceOrder.branchID>(fsServiceOrderRow, crOpportunityRow.BranchID);
                somethingChanged = true;
            }

            if (fsServiceOrderRow.BranchLocationID != fsCreateServiceOrderOnOpportunityFilterRow.BranchLocationID)
            {
                graphServiceOrderEntry.ServiceOrderRecords.SetValueExt<FSServiceOrder.branchLocationID>(fsServiceOrderRow, fsCreateServiceOrderOnOpportunityFilterRow.BranchLocationID);
                somethingChanged = true;
            }

            if (fsServiceOrderRow.ContactID != crOpportunityRow.ContactID)
            {
                graphServiceOrderEntry.ServiceOrderRecords.SetValueExt<FSServiceOrder.contactID>(fsServiceOrderRow, crOpportunityRow.ContactID);
                somethingChanged = true;
            }

            if (fsServiceOrderRow.DocDesc != crOpportunityRow.Subject)
            {
                graphServiceOrderEntry.ServiceOrderRecords.SetValueExt<FSServiceOrder.docDesc>(fsServiceOrderRow, crOpportunityRow.Subject);
                somethingChanged = true;
            }

            if (fsServiceOrderRow.ProjectID != crOpportunityRow.ProjectID)
            {
                graphServiceOrderEntry.ServiceOrderRecords.SetValueExt<FSServiceOrder.projectID>(fsServiceOrderRow, crOpportunityRow.ProjectID);
                somethingChanged = true;
            }

            if (crOpportunityRow.OwnerID != null)
            {
                if (crOpportunityRow.OwnerID != (Guid?)cache.GetValueOriginal<CROpportunity.ownerID>(crOpportunityRow))
                {
                    int? salesPersonID = GetSalesPersonID(graphServiceOrderEntry, crOpportunityRow.OwnerID);

                    if (salesPersonID != null)
                    {
                        graphServiceOrderEntry.ServiceOrderRecords.SetValueExt<FSServiceOrder.salesPersonID>(fsServiceOrderRow, salesPersonID);
                        somethingChanged = true;
                    }
                }
            }

            if (fsServiceOrderRow.OrderDate != crOpportunityRow.CloseDate)
            {
                graphServiceOrderEntry.ServiceOrderRecords.SetValueExt<FSServiceOrder.orderDate>(fsServiceOrderRow, crOpportunityRow.CloseDate);
                somethingChanged = true;
            }

            ApplyChangesfromContactInfo(graphServiceOrderEntry, crContactRow, fsServiceOrderRow, ref somethingChanged);
            ApplyChangesfromAddressInfo(graphServiceOrderEntry, crAddressRow, fsServiceOrderRow, ref somethingChanged);

            if (fsServiceOrderRow.TaxZoneID != crOpportunityRow.TaxZoneID)
            {
                graphServiceOrderEntry.ServiceOrderRecords.SetValueExt<FSServiceOrder.taxZoneID>(fsServiceOrderRow, crOpportunityRow.TaxZoneID);
                somethingChanged = true;
            }

            if (somethingChanged && updatingExistingSO)
            {
                graphServiceOrderEntry.ServiceOrderRecords.Update(fsServiceOrderRow);
            }
        }

        public static FSSrvOrdType GetServiceOrderType(PXGraph graph, string srvOrdType)
        {
            if (string.IsNullOrEmpty(srvOrdType))
            {
                return null;
            }

            return
                PXSelect<FSSrvOrdType,
                    Where<FSSrvOrdType.srvOrdType, Equal<Required<FSSrvOrdType.srvOrdType>>>>
                .Select(graph, srvOrdType);
        }

        private static void ApplyChangesfromAddressInfo(ServiceOrderEntry graphServiceOrderEntry, CRAddress crAddressRow, FSServiceOrder fsServiceOrderRow, ref bool somethingChanged)
        {
            if (crAddressRow == null)
            {
                return;
            }

            if (fsServiceOrderRow.AddressLine1 != crAddressRow.AddressLine1)
            {
                graphServiceOrderEntry.ServiceOrderRecords.SetValueExt<FSServiceOrder.addressLine1>(fsServiceOrderRow, crAddressRow.AddressLine1);
                somethingChanged = true;
            }

            if (fsServiceOrderRow.AddressLine2 != crAddressRow.AddressLine2)
            {
                graphServiceOrderEntry.ServiceOrderRecords.SetValueExt<FSServiceOrder.addressLine2>(fsServiceOrderRow, crAddressRow.AddressLine2);
                somethingChanged = true;
            }

            if (fsServiceOrderRow.AddressLine3 != crAddressRow.AddressLine3)
            {
                graphServiceOrderEntry.ServiceOrderRecords.SetValueExt<FSServiceOrder.addressLine3>(fsServiceOrderRow, crAddressRow.AddressLine3);
                somethingChanged = true;
            }

            if (fsServiceOrderRow.City != crAddressRow.City)
            {
                graphServiceOrderEntry.ServiceOrderRecords.SetValueExt<FSServiceOrder.city>(fsServiceOrderRow, crAddressRow.City);
                somethingChanged = true;
            }

            if (fsServiceOrderRow.CountryID != crAddressRow.CountryID)
            {
                graphServiceOrderEntry.ServiceOrderRecords.SetValueExt<FSServiceOrder.countryID>(fsServiceOrderRow, crAddressRow.CountryID);
                somethingChanged = true;
            }

            if (fsServiceOrderRow.State != crAddressRow.State)
            {
                graphServiceOrderEntry.ServiceOrderRecords.SetValueExt<FSServiceOrder.state>(fsServiceOrderRow, crAddressRow.State);
                somethingChanged = true;
            }

            if (fsServiceOrderRow.PostalCode != crAddressRow.PostalCode)
            {
                graphServiceOrderEntry.ServiceOrderRecords.SetValueExt<FSServiceOrder.postalCode>(fsServiceOrderRow, crAddressRow.PostalCode);
                somethingChanged = true;
            }
        }

        private static void ApplyChangesfromContactInfo(ServiceOrderEntry graphServiceOrderEntry, CRContact crContactRow, FSServiceOrder fsServiceOrderRow, ref bool somethingChanged)
        {
            if (crContactRow == null)
            {
                return;
            }

            if (fsServiceOrderRow.Attention != crContactRow.Title)
            {
                graphServiceOrderEntry.ServiceOrderRecords.SetValueExt<FSServiceOrder.attention>(fsServiceOrderRow, crContactRow.Title);
                somethingChanged = true;
            }

            if (fsServiceOrderRow.EMail != crContactRow.Email)
            {
                graphServiceOrderEntry.ServiceOrderRecords.SetValueExt<FSServiceOrder.eMail>(fsServiceOrderRow, crContactRow.Email);
                somethingChanged = true;
            }

            if (fsServiceOrderRow.Phone1 != crContactRow.Phone1)
            {
                graphServiceOrderEntry.ServiceOrderRecords.SetValueExt<FSServiceOrder.phone1>(fsServiceOrderRow, crContactRow.Phone1);
                somethingChanged = true;
            }

            if (fsServiceOrderRow.Phone2 != crContactRow.Phone2)
            {
                graphServiceOrderEntry.ServiceOrderRecords.SetValueExt<FSServiceOrder.phone2>(fsServiceOrderRow, crContactRow.Phone2);
                somethingChanged = true;
            }

            if (fsServiceOrderRow.Phone3 != crContactRow.Phone3)
            {
                graphServiceOrderEntry.ServiceOrderRecords.SetValueExt<FSServiceOrder.phone3>(fsServiceOrderRow, crContactRow.Phone3);
                somethingChanged = true;
            }

            if (fsServiceOrderRow.Fax != crContactRow.Fax)
            {
                graphServiceOrderEntry.ServiceOrderRecords.SetValueExt<FSServiceOrder.fax>(fsServiceOrderRow, crContactRow.Fax);
                somethingChanged = true;
            }
        }

        private static int? GetSalesPersonID(PXGraph graph, Guid? userID)
        {
            EPEmployee epeEmployeeRow =
                PXSelect<EPEmployee,
                Where<
                    EPEmployee.userID, Equal<Required<EPEmployee.userID>>>>
                .Select(graph, userID);

            if (epeEmployeeRow != null)
            {
                return epeEmployeeRow.SalesPersonID;
            }

            return null;
        }

        public static FSServiceOrder InitNewServiceOrder(string srvOrdType, string sourceType)
        {
            FSServiceOrder fsServiceOrderRow = new FSServiceOrder();
            fsServiceOrderRow.SrvOrdType = srvOrdType;
            fsServiceOrderRow.SourceType = sourceType;

            return fsServiceOrderRow;
        }

        public static FSServiceOrder GetRelatedServiceOrder(PXGraph graph, PXCache chache, IBqlTable crTable, int? soID)
        {
            FSServiceOrder fsServiceOrderRow = null;

            if (soID != null
                    && chache.GetStatus(crTable) != PXEntryStatus.Inserted)
            {
                fsServiceOrderRow =
                    PXSelect<FSServiceOrder,
                    Where<
                        FSServiceOrder.sOID, Equal<Required<FSServiceOrder.sOID>>>>
                        .Select(graph, soID);
            }

            return fsServiceOrderRow;
        }

        #region Opportunity Methods
        public static FSServiceOrder GetServiceOrderLinkedToOpportunity(PXGraph graph, CROpportunity crOpportunityRow)
        {
            if (graph == null || crOpportunityRow == null)
            {
                return null;
            }

            FSServiceOrder fsServiceOrderRow = (FSServiceOrder)PXSelectJoin<FSServiceOrder,
                    InnerJoin<CROpportunity,
                        On<
                            CROpportunity.opportunityID, Equal<FSServiceOrder.sourceRefNbr>,
                            And<FSServiceOrder.sourceType, Equal<FSServiceOrder.sourceType.Opportunity>>>>,
                    Where<
                        CROpportunity.noteID, Equal<Required<CROpportunity.noteID>>>>
                    .Select(graph, crOpportunityRow.NoteID);

            return fsServiceOrderRow;
        }

        public static void InsertFSSODetFromOpportunity(ServiceOrderEntry graphServiceOrder,
                                                        PXCache cacheOpportunityProducts,
                                                        CRSetup crSetupRow,
                                                        CROpportunityProducts crOpportunityProductRow, 
                                                        FSxCROpportunityProducts fsxCROpportunityProductsRow,
                                                        InventoryItem inventoryItemRow)
        {
            if (graphServiceOrder == null
                    || crOpportunityProductRow == null
                    || fsxCROpportunityProductsRow == null
                    || inventoryItemRow == null)
            {
                return;
            }

            if (inventoryItemRow.StkItem == true)
            {
                //Insert a new SODet line
                FSSODetPart fsSODetPartRow = new FSSODetPart();

                UpdateFSSODetFromOpportunity(graphServiceOrder.ServiceOrderDetParts.Cache,
                                            fsSODetPartRow,
                                            crOpportunityProductRow,
                                            fsxCROpportunityProductsRow,
                                            SharedFunctions.GetLineTypeFromInventoryItem(inventoryItemRow));

                SharedFunctions.CopyNotesAndFiles(cacheOpportunityProducts,
                                            graphServiceOrder.ServiceOrderDetParts.Cache,
                                            crOpportunityProductRow, graphServiceOrder.ServiceOrderDetParts.Current,
                                            crSetupRow.CopyNotes,
                                            crSetupRow.CopyFiles);
            }
            else
            {
                //Insert a new SODet line
                FSSODetService fsSODetServiceRow = new FSSODetService();

                UpdateFSSODetFromOpportunity(graphServiceOrder.ServiceOrderDetServices.Cache,
                                                fsSODetServiceRow,
                                                crOpportunityProductRow,
                                                fsxCROpportunityProductsRow,
                                                SharedFunctions.GetLineTypeFromInventoryItem(inventoryItemRow));

                SharedFunctions.CopyNotesAndFiles(cacheOpportunityProducts,
                                            graphServiceOrder.ServiceOrderDetServices.Cache,
                                            crOpportunityProductRow, graphServiceOrder.ServiceOrderDetServices.Current,
                                            crSetupRow.CopyNotes,
                                            crSetupRow.CopyFiles);
            }
        }

        public static void UpdateFSSODetFromOpportunity(PXCache soDetCache, 
                                                        FSSODet fsSODetRow, 
                                                        CROpportunityProducts crOpportunityProductRow, 
                                                        FSxCROpportunityProducts fsxCROpportunityProductsRow, 
                                                        string lineType)
        {
            if (crOpportunityProductRow == null || fsxCROpportunityProductsRow == null)
            {
                return;
            }

            fsSODetRow.SourceNoteID = crOpportunityProductRow.NoteID;
            soDetCache.Current = fsSODetRow = (FSSODet)soDetCache.Insert(fsSODetRow);

            fsSODetRow.LineType = lineType;
            fsSODetRow.InventoryID = crOpportunityProductRow.InventoryID;
            fsSODetRow.IsBillable = crOpportunityProductRow.IsFree == false;

            soDetCache.Current = fsSODetRow = (FSSODet)soDetCache.Update(fsSODetRow);
            fsSODetRow = (FSSODet)soDetCache.CreateCopy(fsSODetRow);

            fsSODetRow.BillingRule = fsxCROpportunityProductsRow.BillingRule;
            fsSODetRow.TranDesc = crOpportunityProductRow.Descr;

            if (crOpportunityProductRow.SiteID != null)
            {
                fsSODetRow.SiteID = crOpportunityProductRow.SiteID;
            }

            fsSODetRow.EstimatedDuration = fsxCROpportunityProductsRow.EstimatedDuration;
            fsSODetRow.EstimatedQty = crOpportunityProductRow.Qty;

            fsSODetRow.CuryUnitPrice = crOpportunityProductRow.CuryUnitPrice;
            fsSODetRow.ManualPrice = crOpportunityProductRow.ManualPrice;

            fsSODetRow.ProjectID = crOpportunityProductRow.ProjectID;
            fsSODetRow.ProjectTaskID = crOpportunityProductRow.TaskID;

            fsSODetRow.CuryUnitCost = crOpportunityProductRow.CuryUnitCost;
            fsSODetRow.ManualCost = crOpportunityProductRow.POCreate;

            fsSODetRow.EnablePO = crOpportunityProductRow.POCreate;
            fsSODetRow.POVendorID = crOpportunityProductRow.VendorID;
            fsSODetRow.POVendorLocationID = fsxCROpportunityProductsRow.VendorLocationID;

            fsSODetRow.TaxCategoryID = crOpportunityProductRow.TaxCategoryID;

            soDetCache.Current = soDetCache.Update(fsSODetRow);
        }
        #endregion
    }
}