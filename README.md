dform.net
=========

A .Net serializatiation library for jquery.dform
see for more infomation: https://github.com/daffl/jquery.dform

basic usage:

<code>
    [DForm(HttpMethod.POST,"submitForm.aspx")]
    
    public class myform
    {
        [DFormInput]
        public string mytext;
        
        [DFormInput(type = DFORM_TYPE.checkbox] 
        public bool mycheckbox;
    }

    public static class main()
    {
        public void Main(string[] argv,int argc)
        {
            myform toSerialize = new myform();
            string dformjson = DFormSerlializer.Serialize(myform);
        }
    }
</code>