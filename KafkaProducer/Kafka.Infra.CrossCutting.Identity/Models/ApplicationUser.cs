using AspNetCore.Identity.Mongo.Model;
using Microsoft.AspNetCore.Identity;

namespace Kafka.Infra.CrossCutting.Identity.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : MongoUser
    {
    }
}
