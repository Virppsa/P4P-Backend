using P4P.Models;

namespace P4P.Data;

public static class DbInitializer
{
    public static readonly Lazy<Comment[]> Comments = new(SeedComments);
    public static readonly Lazy<User[]> Users = new(SeedUsers);
    public static readonly Lazy<Like[]> Likes = new(SeedLikes);
    public static readonly Lazy<Location[]> Locations = new(SeedLocations);
    public static readonly Lazy<Post[]> Posts = new(SeedPosts);

    public static Task SeedP4PContext(IP4PContext context)
    {
        context.Comment.AddRange(Comments.Value);
        context.User.AddRange(Users.Value);
        context.Like.AddRange(Likes.Value);
        context.Location.AddRange(Locations.Value);
        context.Post.AddRange(Posts.Value);

        return context.Instance.SaveChangesAsync();
    }

    private static Location[] SeedLocations()
    {
        return new Location[]
        {
            new()
            {
                Id = 1,
                Description = "Vilniaus Kolegijos valgykla",
                Ratings = new(){"1-1", "1-2"},
                Name = "VIKO valgykla",
                X = 25.14f,
                Y = 24.36f
            },
            new()
            {
                Id = 2,
                Description = "Gelezinio Vilko g. Hesburger",
                Ratings = new(){"1-1", "1-2"},
                Name = "Hesburger",
                X = 26.14f,
                Y = 27.36f
            },
            new()
            {
                Id = 3,
                Description = "Kebabine Didlaukio g. prie autoserviso",
                Ratings = new(){"1-1", "1-2"},
                Name = "Kebabine",
                X = 21.14f,
                Y = 22.36f
            },
            new()
            {
                Id = 4,
                Description = "Ukmerges g. Mcdonalds",
                Ratings = new(){"1-1", "1-2"},
                Name = "Mcdonalds",
                X = 27.14f,
                Y = 28.36f
            },
        };
    }

    private static User[] SeedUsers()
    {
        return new User[]
        {
            new()
            {
                Id = 1,
                Name = "Paulius",
                Email = "Paulius@gmail.com",
                Password = "slaptazodis",
                Verified = true,
            },
            new()
            {
                Id = 2,
                Name = "Rytis",
                Email = "Rytis@gmail.com",
                Password = "slaptazodis",
                Verified = true,
            },
            new()
            {
                Id = 3,
                Name = "Edvinas",
                Email = "Edvinas@gmail.com",
                Password = "slaptazodis",
                Verified = true,
            },
            new()
            {
                Id = 4,
                Name = "Zydrunas",
                Email = "Savickas@gmail.com",
                Password = "slaptazodis",
                Verified = true,
            }
        };
    }

    private static Like[] SeedLikes()
    {
        return new Like[]
        {
            new()
            {
                Id = 1,
                UserId = 2,
                PostId = 4,
            },
            new()
            {
                Id = 2,
                UserId = 3,
                PostId = 2,
            },
            new()
            {
                Id = 3,
                UserId = 2,
                CommentId = 2,
            },
            new()
            {
                Id = 4,
                UserId = 2,
                CommentId = 3,
            },
        };
    }

    private static Comment[] SeedComments()
    {
        return new Comment[]
        {
            new()
            {
               Id = 1,
               Text = "Labai pigu",
               UserId = 2,
               PostId = 2
            },
            new()
            {
                Id = 2,
                Text = "Labai brangu",
                UserId = 3,
                PostId = 3
            },
            new()
            {
                Id = 3,
                Text = "Labai skanu",
                UserId = 3,
                PostId = 2
            },
            new()
            {
                Id = 4,
                Text = "Labai neskanu",
                UserId = 4,
                PostId = 2
            },
        };
    }

    private static Post[] SeedPosts()
    {
        return new Post[]
        {
            new()
            {
                Id = 1,
                Text = "Lorem ipsum",
                LocationId = 3,
                UserId = 3,
            },
            new()
            {
                Id = 2,
                Text = "Lorem ipsum",
                LocationId = 4,
                UserId = 3,
            },
            new()
            {
                Id = 3,
                Text = "Lorem ipsum",
                LocationId = 5,
                UserId = 4,
            },
            new()
            {
                Id = 4,
                Text = "Lorem ipsum",
                LocationId = 2,
                UserId = 4,
            }
        };
    }
}