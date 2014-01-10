using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration.Provider;

namespace NTTData.Redirector.Providers
{
    public class RedirectProviderCollection: ProviderCollection
    {
        public new RedirectProvider this[string name]
        {
            get
            {
                return (base[name] as RedirectProvider);
            }

        }
    }
}
