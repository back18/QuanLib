using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core
{
    public interface IBindable
    {
        public bool IsBound { get; }

        public void Bind();

        public void Unbind();
    }
}
