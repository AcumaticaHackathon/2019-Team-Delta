using PX.Data;
using PX.Objects.AP;
using PX.Objects.CM;
using PX.Objects.CS;
using PX.Objects.GL;
using PX.Objects.IN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackathon
{
    public class APReleaseProcessExtension : PXGraphExtension<APReleaseProcess>
    {
        public delegate List<APRegister> ReleaseInvoiceDel(JournalEntry je, ref APRegister doc, PXResult<APInvoice, CurrencyInfo, Terms, Vendor> res, bool isPrebooking, out List<INRegister> inDocs);

        [PXOverride]
		public virtual List<APRegister> ReleaseInvoice(JournalEntry je, ref APRegister doc, PXResult<APInvoice, CurrencyInfo, Terms, Vendor> res, bool isPrebooking, out List<INRegister> inDocs, ReleaseInvoiceDel del)
		{
			List<APRegister> results = new List<APRegister>();

			using (PXTransactionScope ts = new PXTransactionScope())
			{
					je.RowInserting.AddHandler<GLTran>((sender, e) =>
					{
						GLTran row = e.Row as GLTran;

						if (row != null)
						{
							if (row.Module == BatchModule.AP)
							{
								APTran tranLine = PXSelectReadonly<APTran,
																		Where<APTran.tranType, Equal<Required<APTran.tranType>>,
																			And<APTran.refNbr, Equal<Required<APTran.refNbr>>,
																				And<APTran.lineNbr, Equal<Required<APTran.lineNbr>>>>>>.Select(Base, new object[] { row.TranType, row.RefNbr, row.TranLineNbr });

								if (tranLine != null)
								{
                                    InventoryItem item = PXSelectReadonly<InventoryItem, Where<InventoryItem.inventoryID, Equal<Required<InventoryItem.inventoryID>>>>.Select(je, tranLine.InventoryID);
                                    if(item != null)
                                    {
                                        InventoryItemExtension itemExt = item.GetExtension<InventoryItemExtension>();
                                        if(!string.IsNullOrEmpty(itemExt?.UsrTDEndpoint))
                                        {
                                            try
                                            {

                                            }
                                            catch
                                            {

                                            }
                                        }
                                    }
								}
							}
						}
					});

				results = del(je, ref doc, res, isPrebooking, out inDocs);
				ts.Complete(Base);
			}

			return results;
		}

    }
}
