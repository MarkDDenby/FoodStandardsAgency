using System.Threading.Tasks;
using ServiceClient.Models;

namespace ServiceClient.Contracts
{
    /// <summary>
    /// Represents the Food Standard Agency Service Client
    /// </summary>
    public interface IFoodStandardAgencyServiceClient
    {
        /// <summary>
        /// Gets all authorities.
        /// </summary>
        /// <returns>Task with Authorities result</returns>
        Task<Authorities> GetAllAuthorities();

        /// <summary>
        /// Returns all establishments for a given authority id
        /// </summary>
        /// <returns>Task with Establishments results</returns>
        /// <param name="authorityId">Authority Id for the establishments to be returned</param>
        Task<Establishments> GetEstablishments(int authorityId);

        /// <summary>
        /// Return an authority for a given authority id
        /// </summary>
        /// <returns>Task with an authority result</returns>
        /// <param name="authorityId">Authority identifier.</param>
        Task<Authority> GetAuthority(int authorityId);
    }
}