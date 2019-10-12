using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fikra.DAL.StoredProcedures
{
    public interface IStoredProcBuilder<T> where T : IStoredProcedure, new()
    {
	    IStoredProcBuilder<T> StoredProc();
	    IStoredProcBuilder<T> Input(StoredProcInput input);
	    IStoredProcBuilder<T> Input(IEnumerable<StoredProcInput> input);
	    IStoredProcBuilder<T> As();
	    IStoredProcBuilder<T> Begin();
	    IStoredProcBuilder<T> Select(string statement);
	    IStoredProcBuilder<T> SelectAll();
	    IStoredProcBuilder<T> From(string statement);
	    IStoredProcBuilder<T> Where(string statement);
	    string End();

    }
}
