namespace BackEnd_WebApi.Application.Exeptions
{
    public class NotMachEmailPassExaception : ApplicationException 
    {
        public override string Message => "The Email or password is incorrect";
    }

}
