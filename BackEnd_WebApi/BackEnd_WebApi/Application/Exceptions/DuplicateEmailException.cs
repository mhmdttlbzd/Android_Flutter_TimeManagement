namespace BackEnd_WebApi.Application.Exeptions
{
    public class DuplicateEmailException : ApplicationException 
    {
        public override string Message => "The Email already exists";
    } 

}
