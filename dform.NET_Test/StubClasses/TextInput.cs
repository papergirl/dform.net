

namespace dform.NET_Test.StubClasses
{
    using dform.NET;
    internal class TextInput
    {
        [DFormInput(type = DFORM_TYPE.Text, id = "test_id", name = "test_name", @class = "test_class")]
        public string Text { get; set; }

        public TextInput()
        {
            Text = "Hello world";
        }
        

    }
}
