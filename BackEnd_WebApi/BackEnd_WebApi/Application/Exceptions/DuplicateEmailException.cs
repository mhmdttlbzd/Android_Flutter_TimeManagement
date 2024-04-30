namespace BackEnd_WebApi.Application.Exeptions
{
    public class DuplicateEmailException : MyApplicationException 
    {
        public override string Message => "The Email already exists";
    } 

}
