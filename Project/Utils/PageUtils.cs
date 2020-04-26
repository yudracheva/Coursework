using Project.Models.Documents;
using Project.Pages.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Utils
{
    public static class PageUtils
    {
        public static void ChangeCount(object value, List<LineOfMaterials> materials, int numberLine)
        {
            var line = materials.FirstOrDefault(d => d.Number == numberLine);
            var count = Convert.ToInt32(value);
            line.Count = count < 0 ? 0 : count;
            line.Sum = line.Price * line.Count;
        }

        public static string ChangeDate(object value)
        {
            var date = DateTime.Parse(value.ToString());
            return date.ToString(DefaultComponentBase.DATE_TO_PAGE_STRING_FORMAT);
        }

        internal static void ChangePrice(object value, List<LineOfMaterials> materials, int numberLine)
        {
            var line = materials.FirstOrDefault(d => d.Number == numberLine);
            var price = 0m;

            if (String.IsNullOrEmpty(value.ToString()))
                price = 0;
            else
                price = Convert.ToDecimal(value.ToString().Replace('.', ','));

            line.Price = price < 0 ? 0 : price;
            line.Sum = line.Price * line.Count;
        }

        internal static void ChangeSum(object value, List<LineOfMaterials> materials, int numberLine)
        {
            var line = materials.FirstOrDefault(d => d.Number == numberLine);
            var sum = Convert.ToDecimal(value.ToString().Replace('.', ','));
            line.Sum = sum < 0 ? 0 : sum;
            if (line.Count != 0)
                line.Price = line.Sum / line.Count;
        }
    }
}
