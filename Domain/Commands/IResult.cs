namespace Domain.Commands
{
    public interface IResult <out T>
    {
        T GetResult();
    }
}