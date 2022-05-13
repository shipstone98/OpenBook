using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using Shipstone.OpenBook.Models;
using Shipstone.OpenBook.ViewModels;

namespace Shipstone.OpenBook.Data
{
    public static class FollowingDao
    {
        public static async Task<HttpStatusCode> CreateAsync(
            Context context,
            int followerId,
            int followeeId
        )
        {
            Following following = await FollowingDao.RetrieveFollowingAsync(context, followerId, followeeId);

            if (!(following is null))
            {
                return HttpStatusCode.Conflict;
            }

            following = new Following
            {
                FolloweeId = followeeId,
                FollowerId = followerId
            };

            await context.Followings.AddAsync(following);
            await context.SaveChangesAsync();
            return HttpStatusCode.Created;
        }

        public static async Task<HttpStatusCode> DeleteAsync(
            Context context,
            int followerId,
            int followeeId
        )
        {
            Following following = await FollowingDao.RetrieveFollowingAsync(
                context,
                followerId,
                followeeId,
                true
            );

            if (following is null)
            {
                return HttpStatusCode.NotFound;
            }

            context.Followings.Remove(following);
            await context.SaveChangesAsync();
            return HttpStatusCode.NoContent;
        }

        public static async Task<StatusViewModel<Following>> RetrieveAsync(
            Context context,
            int followerId,
            int followeeId
        )
        {
            Following following = await FollowingDao.RetrieveFollowingAsync(context, followerId, followeeId);

            if (following is null)
            {
                return new StatusViewModel<Following>
                {
                    StatusCode = HttpStatusCode.NotFound
                };
            }

            return new StatusViewModel<Following>
            {
                StatusCode = HttpStatusCode.OK,
                ViewModel = following
            };
        }

        public static async Task<StatusViewModel<HashSet<int>>> RetrieveFolloweesAsync(
            Context context,
            int followerId
        )
        {
            HashSet<int> followeeIds = new();

            IEnumerable<Following> followings = await context.Followings
                .Where(f => f.FollowerId == followerId)
                .AsNoTracking()
                .ToArrayAsync();

            foreach (Following following in followings)
            {
                followeeIds.Add(following.FolloweeId);
            }

            return new StatusViewModel<HashSet<int>>
            {
                StatusCode = HttpStatusCode.OK,
                ViewModel = followeeIds
            };
        }

        private static async Task<Following> RetrieveFollowingAsync(
            Context context,
            int followerId,
            int followeeId,
            bool ignoreNavigationProperties = false
        )
        {
            IQueryable<Following> query;

            if (ignoreNavigationProperties)
            {
                query = context.Followings;
            }

            else
            {
                query = context.Followings
                    .Include(f => f.Followee)
                    .Include(f => f.Follower);
            }

            return await query
                .Where(f => f.FolloweeId == followeeId && f.FollowerId == followerId)
                .AsNoTracking()
                .SingleOrDefaultAsync();
        }
    }
}
