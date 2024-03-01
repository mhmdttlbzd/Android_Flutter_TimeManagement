namespace BackEnd_WebApi.Application.Exeptions
{
    public class IncorrectInputExaception : ApplicationException 
    {
        public override string Message => "Enter the valiues correctly";
    }

}
