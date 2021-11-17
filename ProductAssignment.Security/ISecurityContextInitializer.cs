namespace ProductAssignment.Security
{
    public interface ISecurityContextInitializer
    {
        void Initialize(SecurityContext context);
    }
}