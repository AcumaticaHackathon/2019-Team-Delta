﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PX.CS.Contracts.Interfaces;
using PX.Data;
using PX.Objects.CR.Standalone;
using PX.Objects.CS;
using PX.Objects.GL;
using PX.Objects.IN;
using PX.Objects.TX;
using PX.Objects.TX.GraphExtensions;
using PX.TaxProvider;

namespace PX.Objects.CR
{
	public class QuoteMaintExternalTax : ExternalTaxBase<QuoteMaint, CRQuote>
	{
		public static bool IsActive()
		{
			return PXAccess.FeatureInstalled<FeaturesSet.avalaraTax>();
		}

		[PXOverride]
		public void Persist()
		{
			if (Base.Quote.Current != null && IsExternalTax(Base.Quote.Current.TaxZoneID) && !skipExternalTaxCalcOnSave && Base.Quote.Current.IsTaxValid != true)
			{
				if (PXLongOperation.GetCurrentItem() == null)
				{
					PXLongOperation.StartOperation(Base, delegate ()
					{
						CalculateExternalTax(Base.Quote.Current);
					});
				}
				else
				{
					CalculateExternalTax(Base.Quote.Current);
				}
			}
		}

		protected override void SkipTaxCalcAndSave()
		{
			try
			{
				skipExternalTaxCalcOnSave = true;
				Base.Save.Press();
			}
			finally
			{
				skipExternalTaxCalcOnSave = false;
			}
		}

		protected virtual void _(Events.RowUpdated<CRQuote> e)
		{
			if (IsExternalTax(e.Row.TaxZoneID) && !e.Cache.ObjectsEqual<CRQuote.contactID, CRQuote.taxZoneID, CRQuote.branchID, CRQuote.locationID,
					CRQuote.curyAmount, CRQuote.opportunityAddressID>(e.Row, e.OldRow))
			{
				e.Row.IsTaxValid = false;
			}
		}

		protected virtual void _(Events.RowInserted<CROpportunityProducts> e)
		{
			if (IsExternalTax(Base.Quote.Current.TaxZoneID))
			{
				Base.Quote.Current.IsTaxValid = false;
				if (Base.Quote.Cache.GetStatus(Base.Quote.Current) == PXEntryStatus.Notchanged)
					Base.Quote.Cache.SetStatus(Base.Quote.Current, PXEntryStatus.Updated);
			}
		}
		protected virtual void _(Events.RowUpdated<CROpportunityProducts> e)
		{
			if (IsExternalTax(Base.Quote.Current.TaxZoneID))
			{
				Base.Quote.Current.IsTaxValid = false;
				if (Base.Quote.Cache.GetStatus(Base.Quote.Current) == PXEntryStatus.Notchanged)
					Base.Quote.Cache.SetStatus(Base.Quote.Current, PXEntryStatus.Updated);
			}
		}
		protected virtual void _(Events.RowDeleted<CROpportunityProducts> e)
		{
			if (IsExternalTax(Base.Quote.Current.TaxZoneID))
			{
				Base.Quote.Current.IsTaxValid = false;
				if (Base.Quote.Cache.GetStatus(Base.Quote.Current) == PXEntryStatus.Notchanged)
					Base.Quote.Cache.SetStatus(Base.Quote.Current, PXEntryStatus.Updated);
			}
		}

		protected virtual void _(Events.RowInserted<CROpportunityDiscountDetail> e)
		{
			if (IsExternalTax(Base.Quote.Current.TaxZoneID))
			{
				Base.Quote.Current.IsTaxValid = false;
				if (Base.Quote.Cache.GetStatus(Base.Quote.Current) == PXEntryStatus.Notchanged)
					Base.Quote.Cache.SetStatus(Base.Quote.Current, PXEntryStatus.Updated);
			}
		}
		protected virtual void _(Events.RowUpdated<CROpportunityDiscountDetail> e)
		{
			if (IsExternalTax(Base.Quote.Current.TaxZoneID))
			{
				Base.Quote.Current.IsTaxValid = false;
				if (Base.Quote.Cache.GetStatus(Base.Quote.Current) == PXEntryStatus.Notchanged)
					Base.Quote.Cache.SetStatus(Base.Quote.Current, PXEntryStatus.Updated);
			}
		}
		protected virtual void _(Events.RowDeleted<CROpportunityDiscountDetail> e)
		{
			if (IsExternalTax(Base.Quote.Current.TaxZoneID))
			{
				Base.Quote.Current.IsTaxValid = false;
				if (Base.Quote.Cache.GetStatus(Base.Quote.Current) == PXEntryStatus.Notchanged)
					Base.Quote.Cache.SetStatus(Base.Quote.Current, PXEntryStatus.Updated);
			}
		}

		public override CRQuote CalculateExternalTax(CRQuote quote)
		{
			var toAddress = GetToAddress(quote);
			bool isNonTaxable = IsNonTaxable(toAddress);

			if (isNonTaxable || quote.BAccountID == null)
			{
				ApplyTax(quote, GetTaxResult.Empty);
				quote.IsTaxValid = true;
				quote = Base.Quote.Update(quote);

				SkipTaxCalcAndSave();

				return quote;
			}

			var service = TaxProviderFactory(Base, quote.TaxZoneID);

			GetTaxRequest getRequest = null;
			bool isValidByDefault = true;

			if (quote.IsTaxValid != true)
			{
				getRequest = BuildGetTaxRequest(quote);

				if (getRequest.CartItems.Count > 0)
				{
					isValidByDefault = false;
				}
				else
				{
					getRequest = null;
				}
			}

			if (isValidByDefault)
			{
				quote.IsTaxValid = true;
				quote = Base.Quote.Update(quote);
				SkipTaxCalcAndSave();
				return quote;
			}

			GetTaxResult result = service.GetTax(getRequest);
			if (result.IsSuccess)
			{
				try
				{
					ApplyTax(quote, result);
					quote.IsTaxValid = true;
					quote = Base.Quote.Update(quote);
					SkipTaxCalcAndSave();
				}
				catch (PXOuterException ex)
				{
					string msg = TX.Messages.FailedToApplyTaxes;
					foreach (string err in ex.InnerMessages)
					{
						msg += Environment.NewLine + err;
					}

					throw new PXException(ex, msg);
				}
				catch (Exception ex)
				{
					throw new PXException(ex, TX.Messages.FailedToApplyTaxes);
				}
			}
			else
			{
				LogMessages(result);

				throw new PXException(TX.Messages.FailedToGetTaxes);
			}

			return quote;
		}

		protected GetTaxRequest BuildGetTaxRequest(CRQuote quote)
		{
			if (quote == null)
				throw new PXArgumentException(ErrorMessages.ArgumentNullException);

			BAccount cust = (BAccount)PXSelect<BAccount,
				Where<BAccount.bAccountID, Equal<Required<BAccount.bAccountID>>>>.
				Select(Base, quote.BAccountID);
			Location loc = (Location)PXSelect<Location,
				Where<Location.bAccountID, Equal<Required<Location.bAccountID>>, And<Location.locationID, Equal<Required<Location.locationID>>>>>.
				Select(Base, quote.BAccountID, quote.LocationID);

			IAddressBase addressFrom = GetFromAddress(quote);
			IAddressBase addressTo = GetToAddress(quote);

			if (addressFrom == null)
				throw new PXException(Messages.FailedGetFromAddressCR);

			if (addressTo == null)
				throw new PXException(Messages.FailedGetToAddressCR);

			GetTaxRequest request = new GetTaxRequest();
			request.CompanyCode = CompanyCodeFromBranch(quote.TaxZoneID, Base.Accessinfo.BranchID);
			request.CurrencyCode = quote.CuryID;
			request.CustomerCode = cust?.AcctCD;
			request.OriginAddress = AddressConverter.ConvertTaxAddress(addressFrom);
			request.DestinationAddress = AddressConverter.ConvertTaxAddress(addressTo);
			request.DocCode = string.Format("CR.{0}", quote.OpportunityID);
			request.DocDate = quote.DocumentDate.GetValueOrDefault();
			request.Discount = quote.CuryLineDocDiscountTotal.GetValueOrDefault();

			int mult = 1;

			if (!string.IsNullOrEmpty(loc?.CAvalaraCustomerUsageType))
			{
				request.CustomerUsageType = loc.CAvalaraCustomerUsageType;
			}
			if (!string.IsNullOrEmpty(loc?.CAvalaraExemptionNumber))
			{
				request.ExemptionNo = loc.CAvalaraExemptionNumber;
			}

			request.DocType = TaxDocumentType.SalesOrder;

			var select = PXSelectJoin<CROpportunityProducts,
				LeftJoin<InventoryItem, On<InventoryItem.inventoryID, Equal<CROpportunityProducts.inventoryID>>,
					LeftJoin<Account, On<Account.accountID, Equal<InventoryItem.salesAcctID>>>>,
				Where<CROpportunityProducts.quoteID, Equal<Required<CRQuote.quoteID>>>,
				OrderBy<Asc<CROpportunityProducts.lineNbr>>>.Select(Base, quote.QuoteID);

			foreach (PXResult<CROpportunityProducts, InventoryItem, Account> res in select)
			{
				CROpportunityProducts tran = (CROpportunityProducts)res;
				InventoryItem item = (InventoryItem)res;
				Account salesAccount = (Account)res;

				var line = new TaxCartItem();
				line.Index = tran.LineNbr ?? 0;
				line.Amount = mult * tran.CuryAmount.GetValueOrDefault();
				line.Description = tran.Descr;
				line.DestinationAddress = request.DestinationAddress;
				line.OriginAddress = request.OriginAddress;
				line.ItemCode = item.InventoryCD;
				line.Quantity = tran.Qty.GetValueOrDefault();
				line.Discounted = request.Discount > 0;
				line.RevAcct = salesAccount.AccountCD;
				line.TaxCode = tran.TaxCategoryID;

				request.CartItems.Add(line);
			}

			return request;
		}

		protected void ApplyTax(CRQuote quote, GetTaxResult result)
		{
			TaxZone taxZone = null;
			AP.Vendor vendor = null;

			if (result.TaxSummary.Length > 0)
			{
				taxZone = (TaxZone)PXSetup<TaxZone, Where<TaxZone.taxZoneID, Equal<Required<CROpportunity.taxZoneID>>>>.Select(Base, quote.TaxZoneID);
				vendor = (VendorMaster)PXSelectReadonly<VendorMaster, Where<VendorMaster.bAccountID, Equal<Required<VendorMaster.bAccountID>>>>.Select(Base, taxZone.TaxVendorID);

				if (vendor == null)
					throw new PXException(Messages.ExternalTaxVendorNotFound);
			}

			//Clear all existing Tax transactions:
			foreach (PXResult<CRTaxTran, Tax> res in Base.Taxes.View.SelectMultiBound(new object[] { quote }))
			{
				CRTaxTran taxTran = (CRTaxTran)res;
				Base.Taxes.Delete(taxTran);
			}

			Base.Views.Caches.Add(typeof(Tax));

			for (int i = 0; i < result.TaxSummary.Length; i++)
			{
				string taxID = GetTaxID(result.TaxSummary[i]);

				//Insert Tax if not exists - just for the selectors sake
				Tax tx = PXSelect<Tax, Where<Tax.taxID, Equal<Required<Tax.taxID>>>>.Select(Base, taxID);
				if (tx == null)
				{
					tx = new Tax();
					tx.TaxID = taxID;
					tx.Descr = PXMessages.LocalizeFormatNoPrefixNLA(TX.Messages.ExternalTaxProviderTaxId, taxID);
					tx.TaxType = CSTaxType.Sales;
					tx.TaxCalcType = CSTaxCalcType.Doc;
					tx.TaxCalcLevel = result.TaxSummary[i].TaxCalculationLevel.ToCSTaxCalcLevel();
					tx.TaxApplyTermsDisc = CSTaxTermsDiscount.ToTaxableAmount;
					tx.SalesTaxAcctID = vendor.SalesTaxAcctID;
					tx.SalesTaxSubID = vendor.SalesTaxSubID;
					tx.ExpenseAccountID = vendor.TaxExpenseAcctID;
					tx.ExpenseSubID = vendor.TaxExpenseSubID;
					tx.TaxVendorID = taxZone.TaxVendorID;
					tx.IsExternal = true;

					Base.Caches[typeof(Tax)].Insert(tx);
				}

				CRTaxTran tax = new CRTaxTran();
				tax.QuoteID = quote.QuoteID;
				tax.TaxID = taxID;
				tax.CuryTaxAmt = result.TaxSummary[i].TaxAmount;
				tax.CuryTaxableAmt = result.TaxSummary[i].TaxableAmount;
				tax.TaxRate = Convert.ToDecimal(result.TaxSummary[i].Rate) * 100;

				Base.Taxes.Insert(tax);
			}

			Base.Quote.SetValueExt<CRQuote.curyTaxTotal>(quote, result.TotalTaxAmount);

			decimal? СuryProductsAmount =
				quote.ManualTotalEntry == true
				? quote.CuryAmount - quote.CuryDiscTot
				: quote.CuryLineTotal - quote.CuryDiscTot + quote.CuryTaxTotal;
			Base.Quote.SetValueExt<CRQuote.curyProductsAmount>(quote, СuryProductsAmount ?? 0m);
		}

		protected IAddressBase GetFromAddress(CRQuote quote)
		{
			PXSelectBase<Branch> select = new PXSelectJoin
			<Branch, InnerJoin<BAccount, On<BAccount.bAccountID, Equal<Branch.bAccountID>>,
					InnerJoin<Address, On<Address.addressID, Equal<BAccount.defAddressID>>>>,
				Where<Branch.branchID, Equal<Required<Branch.branchID>>>>(Base);

			foreach (PXResult<Branch, BAccount, Address> res in select.Select(quote.BranchID))
				return (Address)res;

			return null;
		}

		protected IAddressBase GetToAddress(CRQuote quote)
		{
			CRAddress crShipAddress = PXSelect<CRAddress, Where<CRAddress.addressID, Equal<Required<CRAddress.addressID>>>>.Select(Base, quote.OpportunityAddressID);

			if (crShipAddress != null)
				return crShipAddress;

			Address shipAddress = null;

			Location loc = (Location)PXSelect<Location,
					Where<Location.bAccountID, Equal<Required<Location.bAccountID>>, And<Location.locationID, Equal<Required<Location.locationID>>>>>.
				Select(Base, quote.BAccountID, quote.LocationID);

			if (loc != null)
			{
				shipAddress = PXSelect<Address, Where<Address.addressID, Equal<Required<Address.addressID>>>>.Select(Base, loc.DefAddressID);
			}

			return shipAddress;
		}
	}
}
