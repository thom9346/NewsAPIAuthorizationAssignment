namespace Week14Security.Data
{
    public interface IDbInitializer
    {
        void Initialize(ApplicationDbContext context);
    }
}
