using Supabase;

namespace Network.Supabase.Core
{
    public interface ISupabaseManager
    {
        Client Supabase();
    }
}