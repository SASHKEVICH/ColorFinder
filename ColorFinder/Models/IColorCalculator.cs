using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;

namespace ColorFinder.Models
{
    public interface IColorCalculator
    {
        public Task<List<Color>> GetDominantColors();
    }
}