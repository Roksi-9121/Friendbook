using MongoDB.Bson;
using MongoDB.Driver;
using SocialApp.Models;
using System;
using System.Linq;

namespace SocialApp.Services
{
    public class UserService
    {
        private readonly DatabaseService _dbService;

        public UserService(DatabaseService dbService)
        {
            _dbService = dbService;
        }

        public void Register(User user)
        {
            var existingUser = _dbService.Users.Find(u => u.Email == user.Email).FirstOrDefault();
            if (existingUser != null)
            {
                Console.WriteLine("User with this email already exists.");
            }
            else
            {
                _dbService.Users.InsertOne(user);
                Console.WriteLine("User registered successfully.");
            }
        }


        public User Login(string email, string password)
        {
            var user = _dbService.Users.Find(u => u.Email == email && u.Password == password).FirstOrDefault();
            if (user != null)
            {
                Console.WriteLine("Login successful.");
                return user;
            }
            else
            {
                Console.WriteLine("Login failed. Incorrect email or password.");
                return null;
            }
        }

        public void ViewStream()
        {
            var posts = _dbService.Posts.Find(_ => true)
                                          .SortByDescending(p => p.CreatedAt)
                                          .ToList();

            if (posts.Count == 0)
            {
                Console.WriteLine("No posts available.");
                return;
            }

            foreach (var post in posts)
            {
                var user = _dbService.Users.Find(u => u.Id == post.UserId).FirstOrDefault();

                if (user != null)
                {
                    Console.WriteLine($"{user.FirstName} {user.LastName}: {post.Content} (Posted on {post.CreatedAt})");
                }
                else
                {
                    Console.WriteLine($"User with ID {post.UserId} not found for post: {post.Content} (Posted on {post.CreatedAt})");
                }
            }
        }


        public void ViewPosts(User loggedInUser)
        {
            var posts = _dbService.Posts.Find(p => p.UserId == loggedInUser.Id)
                                        .SortByDescending(p => p.CreatedAt)
                                        .ToList();

            foreach (var post in posts)
            {
                Console.WriteLine($"{post.Content} (Posted on {post.CreatedAt})");
            }
        }

        public void AddFriend(User loggedInUser, string friendEmail)
        {
            var friend = _dbService.Users.Find(u => u.Email == friendEmail).FirstOrDefault();
            if (friend != null)
            {
                if (!loggedInUser.Friends.Contains(friend.Id))
                {
                    loggedInUser.Friends.Add(friend.Id);
                    _dbService.Users.ReplaceOne(u => u.Id == loggedInUser.Id, loggedInUser);
                    Console.WriteLine("Friend added successfully.");
                }
                else
                {
                    Console.WriteLine("This user is already your friend.");
                }
            }
            else
            {
                Console.WriteLine("User not found.");
            }
        }

        public void ReactToPost(ObjectId postId, User loggedInUser, string reactionType)
        {
            var post = _dbService.Posts.Find(p => p.Id == postId).FirstOrDefault();
            if (post != null)
            {
                var existingReaction = post.Reactions.FirstOrDefault(r => r.UserId == loggedInUser.Id);

                if (existingReaction != null)
                {
                    existingReaction.ReactionType = reactionType; 
                }
                else
                {
                    var newReaction = new Reaction
                    {
                        UserId = loggedInUser.Id,
                        ReactionType = reactionType
                    };
                    post.Reactions.Add(newReaction);
                }

                _dbService.Posts.ReplaceOne(p => p.Id == postId, post);
                Console.WriteLine("Reaction added/updated.");
            }
            else
            {
                Console.WriteLine("Post not found.");
            }
        }


        public void RemoveFriend(User currentUser, string friendEmail)
        {
            var friend = _dbService.Users.Find(u => u.Email == friendEmail).FirstOrDefault();

            if (friend == null)
            {
                Console.WriteLine("No user found with this email.");
                return;
            }

            if (!currentUser.Friends.Contains(friend.Id))
            {
                Console.WriteLine("This user is not your friend.");
                return;
            }

            currentUser.Friends.Remove(friend.Id);

            _dbService.Users.ReplaceOne(u => u.Id == currentUser.Id, currentUser);

            Console.WriteLine($"Removed friend with email: {friendEmail} from your friends.");
        }

        public void ViewAndCommentOnUserPosts(User currentUser, string userEmail)
        {
            var user = _dbService.Users.Find(u => u.Email == userEmail).FirstOrDefault();

            if (user == null)
            {
                Console.WriteLine("User not found.");
                return;
            }

            var posts = _dbService.Posts.Find(p => p.UserId == user.Id)
                                        .SortByDescending(p => p.CreatedAt)
                                        .ToList();

            if (posts.Count == 0)
            {
                Console.WriteLine("This user has no posts.");
                return;
            }

            foreach (var post in posts)
            {
                Console.WriteLine($"{post.Content} (Posted on {post.CreatedAt})");

                if (post.Comments.Any())
                {
                    Console.WriteLine("Comments:");
                    foreach (var comment in post.Comments)
                    {
                        var commentUser = _dbService.Users.Find(u => u.Id == comment.UserId).FirstOrDefault();
                        if (commentUser != null)
                        {
                            Console.WriteLine($"{commentUser.FirstName} {commentUser.LastName}: {comment.Content} (Commented on {comment.CreatedAt})");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("No comments.");
                }

                Console.WriteLine("Would you like to add a comment to this post? (yes/no)");
                var input = Console.ReadLine();
                if (input?.ToLower() == "yes")
                {
                    Console.WriteLine("Enter your comment:");
                    var commentContent = Console.ReadLine();

                    var newComment = new Comment
                    {
                        UserId = currentUser.Id,
                        Content = commentContent,
                        CreatedAt = DateTime.Now
                    };

                    post.Comments.Add(newComment);

                    _dbService.Posts.ReplaceOne(p => p.Id == post.Id, post);

                    Console.WriteLine("Comment added.");
                }
            }
        }
  

        public void AddPost(User loggedInUser)
        {
            Console.WriteLine("Enter the content of your post:");
            var content = Console.ReadLine();

            var newPost = new Post
            {
                Id = ObjectId.GenerateNewId(),
                UserId = loggedInUser.Id,
                Content = content,
                CreatedAt = DateTime.Now,
                Reactions = new List<Reaction>(), 
                Comments = new List<Comment>()
            };

            _dbService.Posts.InsertOne(newPost);

            Console.WriteLine("Post added successfully.");
        }

    }
}
