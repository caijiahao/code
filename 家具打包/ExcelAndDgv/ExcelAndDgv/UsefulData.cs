using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelAndDgv
{
   public class UsefulData
    {
        private int number;
        private double length;
        private double weight;
        private double thick;

        public UsefulData(int num,double len, double wei, double thi)
        {
            number = num;
            length = len;
            weight = wei;
            thick = thi;
        }

        public override string ToString() //重写ToString方法作为测试
        {
            return string.Format("该部件的数据为:{0},  {1},  {2},  {3}", number, length, weight, thick);
        }
      
    }
}
