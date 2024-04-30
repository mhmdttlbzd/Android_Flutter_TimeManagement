namespace BackEnd_WebApi.Application.Exeptions
{
    public class IncorrectInputExaception : MyApplicationException 
    {
        public override string Message => "Enter the valiues correctly";
    }

}
