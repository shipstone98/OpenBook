using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using Shipstone.OpenBook.Models;
using Shipstone.OpenBook.ViewModels;

namespace Shipstone.OpenBook.Data
{
    public static class PostDao
    {
        public static async Task<StatusViewModel<int>> CreateAsync(Context context, User user, Post model)
        {
            DateTime utcNow = DateTime.UtcNow;

            Post post = new Post
            {
                Content = model.Content,
                CreatedUtc = utcNow,
                CreatorId = user.Id,
                ModifiedUtc = utcNow
            };

            await context.Posts.AddAsync(post);
            await context.SaveChangesAsync();

            return new StatusViewModel<int>
            {
                StatusCode = HttpStatusCode.Created,
                ViewModel = post.Id
            };
        }

        public static async Task<HttpStatusCode> DeleteAsync(Context context, User user, int id)
        {
            Post post = await PostDao.RetrievePostAsync(context, id);

            if (post is null)
            {
                return HttpStatusCode.NotFound;
            }

            if (post.CreatorId != user.Id)
            {
                return HttpStatusCode.Forbidden;
            }

            context.Posts.Remove(post);
            await context.SaveChangesAsync();
            return HttpStatusCode.NoContent;
        }

        public static async Task<StatusViewModel<Post>> RetrieveAsync(Context context, int id)
        {
            Post post = await PostDao.RetrievePostAsync(context, id);
            
            if (post is null)
            {
                return new StatusViewModel<Post>
                {
                    StatusCode = HttpStatusCode.NotFound
                };
            }

            return new StatusViewModel<Post>
            {
                StatusCode = HttpStatusCode.OK,
                ViewModel = post
            };
        }

        public static async Task<StatusViewModel<List<Post>>> RetrieveAllAsync(
            Context context,
            User user,
            bool includeFollowees = false
        )
        {
            if (user is null)
            {
                return new StatusViewModel<List<Post>>
                {
                    StatusCode = HttpStatusCode.NotFound
                };
            }

            List<Post> posts;

            if (includeFollowees)
            {
                ICollection<int> followeeIds = (await FollowingDao.RetrieveFolloweesAsync(context, user.Id)).ViewModel;
                followeeIds.Add(user.Id);
                
                posts = await context.Posts
                    .Include(p => p.Creator)
                    .Where(p => followeeIds.Contains(p.CreatorId))
                    .OrderByDescending(p => p.CreatedUtc)
                    .AsNoTracking()
                    .ToListAsync();
            }

            else
            {
                posts = await context.Posts
                    .Include(p => p.Creator)
                    .Where(p => p.CreatorId == user.Id)
                    .OrderByDescending(p => p.CreatedUtc)
                    .AsNoTracking()
                    .ToListAsync();
            }

            return new StatusViewModel<List<Post>>
            {
                StatusCode = HttpStatusCode.OK,
                ViewModel = posts
            };
        }

        private static async Task<Post> RetrievePostAsync(Context context, int id)
        {
            return await context.Posts
                .Include(p => p.Creator)
                .Where(p => p.Id == id)
                .AsNoTracking()
                .SingleOrDefaultAsync();
        }

        public static async Task<HttpStatusCode> UpdateAsync(
            Context context,
            User user,
            int id,
            Post model
        )
        {
            Post post = await PostDao.RetrievePostAsync(context, id);

            if (post is null)
            {
                return HttpStatusCode.NotFound;
            }

            if (post.CreatorId != user.Id)
            {
                return HttpStatusCode.Forbidden;
            }

            post.Content = model.Content;
            post.ModifiedUtc = DateTime.UtcNow;
            await context.SaveChangesAsync();
            return HttpStatusCode.NoContent;
        }
    }
}
