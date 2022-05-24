using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;

namespace ColorFinder.Models.ColorCalculator
{
    public interface IColorCalculatorMethod
    {
        public Task<List<Color>> GetDominantColors();
        
        public List<Color> ImageColors { set; }
    }
}