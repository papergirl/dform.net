using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dform.NET_Test.StubClasses
{
    using dform.NET;

    internal class CheckboxInput
    {   
        [dform.NET.DFormInput(caption = "checkbox1",
                    @class = "checkbox_class",
                    id = "checkbox1",
                    name = "checkboxes_test",
                    type = DFORM_TYPE.Checkbox)]
        public bool Checkbox1 { get; set; }

        [dform.NET.DFormInput(caption = "checkbox2",
                    @class = "checkbox_class",
                    id = "checkbox2",
                    name = "checkboxes_test",
                    type = DFORM_TYPE.Checkbox)]
        public bool Checkbox2 { get; set; }
    
        public CheckboxInput()
        {
            Checkbox1 = true;
            Checkbox2 = false;
        }
    }
}
