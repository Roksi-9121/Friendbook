using MongoDB.Bson;
using SocialApp.Models;
using SocialApp.Services;
using System;

namespace SocialApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var dbService = new DatabaseService();
            var userService = new UserService(dbService);

            bool exitApp = false;

            while (!exitApp)
            {
                Console.Clear();
                Console.WriteLine("Main Menu:");
                Console.WriteLine("1. Register");
                Console.WriteLine("2. Login");
                Console.WriteLine("0. Exit");
                Console.Write("Choose an option: ");
                var option = Console.ReadLine();

                User loggedInUser = null;

                switch (option)
                {
                    case "1":
                        Console.WriteLine("Enter email:");
                        var email = Console.ReadLine();
                        Console.WriteLine("Enter password:");
                        var password = Console.ReadLine();
                        Console.WriteLine("Enter first name:");
                        var firstName = Console.ReadLine();
                        Console.WriteLine("Enter last name:");
                        var lastName = Console.ReadLine();

                        var newUser = new User
                        {
                            Email = email,
                            Password = password,
                            FirstName = firstName,
                            LastName = lastName
                        };

                        userService.Register(newUser);
                        break;

                    case "2":
                        Console.WriteLine("Enter email:");
                        var loginEmail = Console.ReadLine();
                        Console.WriteLine("Enter password:");
                        var loginPassword = Console.ReadLine();

                        loggedInUser = userService.Login(loginEmail, loginPassword);

                        if (loggedInUser != null)
                        {
                            bool userLoggedIn = true;
                            while (userLoggedIn)
                            {
                                Console.Clear();
                                Console.WriteLine($"Welcome, {loggedInUser.FirstName} {loggedInUser.LastName}!");
                                Console.WriteLine("1. View Stream");
                                Console.WriteLine("2. View My Posts");
                                Console.WriteLine("3. Add Post");
                                Console.WriteLine("4. Add Friend");
                                Console.WriteLine("5. React to Post");
                                Console.WriteLine("6. Remove Friends");
                                Console.WriteLine("7. View and comment post other user");
                                Console.WriteLine("0. Logout");
                                Console.Write("Choose an option: ");
                                var userChoice = Console.ReadLine();

                                switch (userChoice)
                                {
                                    case "1":
                                        userService.ViewStream();
                                        break;
                                    case "2":
                                        userService.ViewPosts(loggedInUser);
                                        break;
                                    case "3":
                                        userService.AddPost(loggedInUser);
                                        break;
                                    case "4":
                                        Console.WriteLine("Enter friend's email:");
                                        var friendEmail = Console.ReadLine();
                                        userService.AddFriend(loggedInUser, friendEmail);
                                        break;
                                    case "5":
                                        Console.WriteLine("Enter post ID:");
                                        var postReactId = ObjectId.Parse(Console.ReadLine());
                                        Console.WriteLine("Enter reaction:");
                                        var reaction = Console.ReadLine();
                                        userService.ReactToPost(postReactId, loggedInUser, reaction);
                                        break;
                                    case "6":
                                        Console.WriteLine("Enter friend's Email to remove:");
                                        var friend_email = Console.ReadLine();
                                        userService.RemoveFriend(loggedInUser, friend_email);
                                        break;
                                    case "7":
                                        Console.WriteLine("Enter User email to view and comment posts:");
                                        var post_email = Console.ReadLine();
                                        userService.ViewAndCommentOnUserPosts(loggedInUser, post_email);
                                        break;
                                    case "0":
                                        userLoggedIn = false;
                                        break;
                                    default:
                                        Console.WriteLine("Invalid option. Please try again.");
                                        break;
                                }

                                if (userLoggedIn)
                                {
                                    Console.WriteLine("Press any key to return to the menu...");
                                    Console.ReadKey();
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("Login failed. Returning to main menu...");
                        }
                        break;

                    case "0":
                        exitApp = true;
                        break;

                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }

                if (!exitApp)
                {
                    Console.WriteLine("Press any key to return to the main menu...");
                    Console.ReadKey();
                }
            }

            Console.WriteLine("Goodbye!");
        }
    }
}
