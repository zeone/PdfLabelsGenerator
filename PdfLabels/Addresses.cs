using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfLabels
{
    public class Addresses
    {
        public static List<string> GetAddresses()
        {
            var resp = new List<string>();
            StringBuilder sb;

            for (int i = 0; i < 15; i++)
            {
                // 3 row
                sb = new StringBuilder();
                sb.Append("3 row Name");
                sb.Append(Environment.NewLine);
                sb.Append("Some Address");
                sb.Append(Environment.NewLine);
                sb.Append("Kyiv, LA 02139");
                resp.Add(sb.ToString());

                // 4 row
                sb = new StringBuilder();
                sb.Append("4 row Name");
                sb.Append(Environment.NewLine);
                sb.Append("Some Company Name");
                sb.Append(Environment.NewLine);
                sb.Append("Some Address");
                sb.Append(Environment.NewLine);
                sb.Append("Kyiv, LA 02139");
                resp.Add(sb.ToString());

                // 5 row
                sb = new StringBuilder();
                sb.Append("5 row Name");
                sb.Append(Environment.NewLine);
                sb.Append("Some Company Name");
                sb.Append(Environment.NewLine);
                sb.Append("Some Address");
                sb.Append(Environment.NewLine);
                sb.Append("Kyiv, LA 02139");
                sb.Append(Environment.NewLine);
                sb.Append("Ukraine");
                resp.Add(sb.ToString());

                // 6 row
                sb = new StringBuilder();
                sb.Append("6 row Name");
                sb.Append(Environment.NewLine);
                sb.Append("Some Company Name");
                sb.Append(Environment.NewLine);
                sb.Append("Some Address");
                sb.Append(Environment.NewLine);
                sb.Append("Kyiv, LA 02139");
                sb.Append(Environment.NewLine);
                sb.Append("Ukraine");
                sb.Append(Environment.NewLine);
                sb.Append("Extra row");
                resp.Add(sb.ToString());
            }

            return resp;
        }
    }
}
