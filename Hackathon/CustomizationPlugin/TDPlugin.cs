using PX.Data;
using PX.Objects.IN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackathon
{
    public class TDPlugin
    {
        public static void UpdateDatabase(TDCustomizationPlugin plugin, ref List<Tuple<string, object[]>> errors)
        {
            string companyName = PXAccess.GetCompanyName();

            NonStockItemMaint graph = PXGraph.CreateInstance<NonStockItemMaint>();
            InventoryItem item;
            INItemClass itemClass;

            try
            {
                itemClass = PXSelectReadonly<INItemClass, Where<INItemClass.itemClassCD, Equal<Required<INItemClass.itemClassCD>>>>.Select(graph, "EXPENSES");
                if(itemClass != null)
                {
                    item = graph.Item.Insert();
                    item.InventoryCD = "CLEANROOM";
                    item = graph.Item.Update(item);
                    item.ItemClassID = itemClass.ItemClassID;
                    item = graph.ItemSettings.Update(item);
                    item.TaxCategoryID = "EXEMPT";
                    item = graph.ItemSettings.Update(item);
                    item.Descr = "House Cleaning Service";
                    item = graph.ItemSettings.Update(item);
                    graph.Save.Press();

                    graph.Clear();
                }
            }
            catch (Exception)
            {
                errors.Add(new Tuple<string, object[]>(TDMessages.ErrorInsertingItems, new object[] { companyName }));
            }
        }
    }
}
