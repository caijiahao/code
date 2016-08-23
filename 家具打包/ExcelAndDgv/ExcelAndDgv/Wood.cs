using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelAndDgv
{
    public class Wood : IComparable
    {

        public double width { get; set; }
        //数量
        public int num { get; set; }
        public double length { get; set; }
        //厚度
        public double thickness { get; set; }
        public int id { get; set; }
        public int packageNumber { get; set; }
        public int range { get; set; }
        public double value { get; set; }


        public Wood(int id,double width,double length,double thickness,int num)
        {
            this.id = id;
            this.width = width;
            this.length = length;
            this.thickness = thickness;
            this.num = num;
        }
        public int CompareTo(object obj)
        {
            int result;
            try
            {
                Wood wood = obj as Wood;
                if (this.id < wood.id)
                {
                    result = -1;
                }
                else
                    result = 1;
                return result;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
    }
    
}
