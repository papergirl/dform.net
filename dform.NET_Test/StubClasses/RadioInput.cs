
namespace dform.NET_Test.StubClasses
{
    using dform.NET;

    internal class RadioInput
    {
        [dform.NET.DFormInput(caption = "radio1",
                              @class = "radiotype",
                              id = "radio_test",
                              name = "radio_test",
                              type = DFORM_TYPE.Radio)]
        public bool radio1 { get; set; }
        [dform.NET.DFormInput(caption = "radio2",
                             @class = "radiotype",
                             id = "radio_test_1",
                             name = "radio_test",
                             type = DFORM_TYPE.Radio)]
        public bool radio2 { get; set; }
       
        public RadioInput()
        {
            radio1 = true;
            radio2 = false;
        }

    }
}
