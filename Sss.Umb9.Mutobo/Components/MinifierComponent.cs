using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.WebAssets;

namespace Sss.Umb9.Mutobo.Components
{
    public class MinifierComponent : IComponent
    {
        private readonly IRuntimeMinifier _runtimeMinifier;

        public MinifierComponent(IRuntimeMinifier runtimeMinifier) => _runtimeMinifier = runtimeMinifier;
        public void Initialize()
        {

            try
            {
                //_runtimeMinifier.CreateJsBundle("inline-js-bundle1",
                //    BundlingOptions.OptimizedAndComposite,
                //    new[] { "" });

                _runtimeMinifier.CreateCssBundle("inline-css-bundle1",
                    BundlingOptions.OptimizedAndComposite,
                    new[] { "~/css/styles.css" });
            }
            catch (Exception ex)
            { 
                
            }

        }

        public void Terminate()
        {

        }
    }
}
