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
        public static void ChangeCount(object value, List<LineOfMaterials> materials, int numberLine, bool checkMimus = true)
        {
            var line = materials.FirstOrDefault(d => d.Number == numberLine);
            var count = Convert.ToInt32(value);
            if (checkMimus)
                count = count < 0 ? 0 : count;

            line.Count = count;
            line.Sum = line.Price * line.Count;
        }

        public static string ChangeDate(object value)
        {
            var date = DateTime.Parse(value.ToString());
            return date.ToString(DocumentComponentBase.DATE_TO_PAGE_STRING_FORMAT);
        }

        public static void ChangePrice(object value, List<LineOfMaterials> materials, int numberLine)
        {
            var line = materials.FirstOrDefault(d => d.Number == numberLine);
            var price = 0m;

            if (String.IsNullOrEmpty(value.ToString()))
                price = 0;
            else
                price = Convert.ToDecimal(value.ToString());

            line.Price = Math.Round(price < 0 ? 0 : price, 2);
            line.Sum = line.Price * line.Count;
        }

        public static void ChangeSum(object value, List<LineOfMaterials> materials, int numberLine)
        {
            var line = materials.FirstOrDefault(d => d.Number == numberLine);
            var sum = Convert.ToDecimal(value.ToString());
            line.Sum = sum < 0 ? 0 : sum;
            if (line.Count != 0)
                line.Price = Math.Round(line.Sum / line.Count, 2);
        }

        public static void RemoveLine(List<LineOfMaterials> materials, int number)
        {
            materials.Remove(materials.FirstOrDefault(d => d.Number.Equals(number)));
            ChangesNumbers(materials);
        }

        public static void AddLine(List<LineOfMaterials> materials, int number)
        {
            materials.Add(new LineOfMaterials());
            ChangesNumbers(materials);
        }

        private static void ChangesNumbers(List<LineOfMaterials> materials)
        {
            for (int i = 0; i < materials.Count; i++)
            {
                materials[i].Number = i + 1;
            }
        }
    }
}
