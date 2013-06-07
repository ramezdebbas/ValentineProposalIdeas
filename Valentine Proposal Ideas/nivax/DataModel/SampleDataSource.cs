using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.ApplicationModel.Resources.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

// The data model defined by this file serves as a representative example of a strongly-typed
// model that supports notification when members are added, removed, or modified.  The property
// names chosen coincide with data bindings in the standard item templates.
//
// Applications may use this model as a starting point and build on it, or discard it entirely and
// replace it with something appropriate to their needs.

namespace PlanningDairyTemplate.Data
{
    /// <summary>
    /// Base class for <see cref="SampleDataItem"/> and <see cref="SampleDataGroup"/> that
    /// defines properties common to both.
    /// </summary>
    [Windows.Foundation.Metadata.WebHostHidden]
    public abstract class SampleDataCommon : PlanningDairyTemplate.Common.BindableBase
    {
        private static Uri _baseUri = new Uri("ms-appx:///");

        public SampleDataCommon(String uniqueId, String title, String subtitle, String imagePath, String description)
        {
            this._uniqueId = uniqueId;
            this._title = title;
            this._subtitle = subtitle;
            this._description = description;
            this._imagePath = imagePath;
        }

        private string _uniqueId = string.Empty;
        public string UniqueId
        {
            get { return this._uniqueId; }
            set { this.SetProperty(ref this._uniqueId, value); }
        }

        private string _title = string.Empty;
        public string Title
        {
            get { return this._title; }
            set { this.SetProperty(ref this._title, value); }
        }

        private string _subtitle = string.Empty;
        public string Subtitle
        {
            get { return this._subtitle; }
            set { this.SetProperty(ref this._subtitle, value); }
        }

        private string _description = string.Empty;
        public string Description
        {
            get { return this._description; }
            set { this.SetProperty(ref this._description, value); }
        }

        private ImageSource _image = null;
        private String _imagePath = null;
        public ImageSource Image
        {
            get
            {
                if (this._image == null && this._imagePath != null)
                {
                    this._image = new BitmapImage(new Uri(SampleDataCommon._baseUri, this._imagePath));
                }
                return this._image;
            }

            set
            {
                this._imagePath = null;
                this.SetProperty(ref this._image, value);
            }
        }

        public void SetImage(String path)
        {
            this._image = null;
            this._imagePath = path;
            this.OnPropertyChanged("Image");
        }
    }

    /// <summary>
    /// Generic item data model.
    /// </summary>
    public class SampleDataItem : SampleDataCommon
    {
        public SampleDataItem(String uniqueId, String title, String subtitle, String imagePath, String description, String content, SampleDataGroup group)
            : base(uniqueId, title, subtitle, imagePath, description)
        {
            this._content = content;
            this._group = group;
        }

        private string _content = string.Empty;
        public string Content
        {
            get { return this._content; }
            set { this.SetProperty(ref this._content, value); }
        }

        private SampleDataGroup _group;
        public SampleDataGroup Group
        {
            get { return this._group; }
            set { this.SetProperty(ref this._group, value); }
        }
        private string _createdon = string.Empty;
        public string CreatedOn
        {
            get { return this._createdon; }
            set { this.SetProperty(ref this._createdon, value); }
        }
        private string _createdtxt = string.Empty;
        public string CreatedTxt
        {
            get { return this._createdtxt; }
            set { this.SetProperty(ref this._createdtxt, value); }
        }

        private string _Colour = string.Empty;
        public string Colour
        {
            get { return this._Colour; }
            set { this.SetProperty(ref this._Colour, value); }
        }
        private string _bgColour = string.Empty;
        public string bgColour
        {
            get { return this._bgColour; }
            set { this.SetProperty(ref this._bgColour, value); }
        }
        private string _createdontwo = string.Empty;
        public string CreatedOnTwo
        {
            get { return this._createdontwo; }
            set { this.SetProperty(ref this._createdontwo, value); }
        }
        private string _createdtxttwo = string.Empty;
        public string CreatedTxtTwo
        {
            get { return this._createdtxttwo; }
            set { this.SetProperty(ref this._createdtxttwo, value); }
        }

        private string _currentStatus = string.Empty;
        public string CurrentStatus
        {
            get { return this._currentStatus; }
            set { this.SetProperty(ref this._currentStatus, value); }
        }
    }

    /// <summary>
    /// Generic group data model.
    /// </summary>
    public class SampleDataGroup : SampleDataCommon
    {
        public SampleDataGroup(String uniqueId, String title, String subtitle, String imagePath, String description)
            : base(uniqueId, title, subtitle, imagePath, description)
        {
        }

        private ObservableCollection<SampleDataItem> _items = new ObservableCollection<SampleDataItem>();
        public ObservableCollection<SampleDataItem> Items
        {
            get { return this._items; }
        }
        
        public IEnumerable<SampleDataItem> TopItems
        {
            // Provides a subset of the full items collection to bind to from a GroupedItemsPage
            // for two reasons: GridView will not virtualize large items collections, and it
            // improves the user experience when browsing through groups with large numbers of
            // items.
            //
            // A maximum of 12 items are displayed because it results in filled grid columns
            // whether there are 1, 2, 3, 4, or 6 rows displayed
            get { return this._items.Take(12); }
        }
    }

    /// <summary>
    /// Creates a collection of groups and items with hard-coded content.
    /// </summary>
    public sealed class SampleDataSource
    {
        private static SampleDataSource _sampleDataSource = new SampleDataSource();

        private ObservableCollection<SampleDataGroup> _allGroups = new ObservableCollection<SampleDataGroup>();
        public ObservableCollection<SampleDataGroup> AllGroups
        {
            get { return this._allGroups; }
        }

        public static IEnumerable<SampleDataGroup> GetGroups(string uniqueId)
        {
            if (!uniqueId.Equals("AllGroups")) throw new ArgumentException("Only 'AllGroups' is supported as a collection of groups");
            
            return _sampleDataSource.AllGroups;
        }

        public static SampleDataGroup GetGroup(string uniqueId)
        {
            // Simple linear search is acceptable for small data sets
            var matches = _sampleDataSource.AllGroups.Where((group) => group.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public static SampleDataItem GetItem(string uniqueId)
        {
            // Simple linear search is acceptable for small data sets
            var matches = _sampleDataSource.AllGroups.SelectMany(group => group.Items).Where((item) => item.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public SampleDataSource()
        {
           // String ITEM_CONTENT = String.Format("");

            var group1 = new SampleDataGroup("Group-1",
                    "Ideas & Directions",
                    "Ideas & Directions",
                    "Assets/Images/10.jpg",
                    "The one thing that gives everyone a shiver down the spine, is proposing someone. You are quite fearful of the fact that how the other person would take it. While guys are much stronger when they are rejected, girls do have a hard time when they propose their beloved and are rejected. Hence it is quite necessary to go through the right path and say it the right way to the other person so that he does not take it in the wrong expression and reject you all together. Hence there are certain rules that need to be kept in mind while you are proposing that man from Mars or that lovely lady from Venus. The ideas, hence, play an important role in making sure how the person would take it.");
            group1.Items.Add(new SampleDataItem("Group-1-Item-1",
                    "Proposing a Girl",
                    "Do a little homework. Get to know your girl well, before you pop the question. Get to know her interests, her dislikes, and her general nature that will help you plan your proposal in a better way. If she is shy and an introvert, propose when you two are alone.",
                    "Assets/DarkGray.png",
					"",            
                    "Details:\n\nDo a little homework. Get to know your girl well, before you pop the question. Get to know her interests, her dislikes, and her general nature that will help you plan your proposal in a better way. If she is shy and an introvert, propose when you two are alone. If she is sporty and loves adventure, then take her for rock climbing and propose when you reach at the top. The age old way of going down on knees in public is still the best way to propose a girl. Don’t try this idea with a shy and introvert girl as she might find it difficult to reciprocate as it could be embarrassing. Words matter a lot to girls if said genuinely or else they can easily figure out the topping of gloss you have added and will be unaffected. If you can write well, write a poem for her and record it in a CD. Play it while you are together and see her melting. All girls love flowers. So get a huge bouquet of roses and tell her how much you love her and how much she means to you. Tell her you would like to spend the rest of your life with her or alone. Be ready to be smothered by a genuinely happy woman's hug! Gestures have a great role to play in proposing a girl as they take a quick notice of it.\n\n Show your gentle, protective and emotional dependence nature while you are around her and mean it too. Girls fall for guys with these qualities and there will be no ground for rejection when you will propose her. Appreciate her for what she is. Tell her you look beautiful when she does not have any make up on. Tell her she looks amazing when she is having a bad day and mean it. She will love you for it and will readily agree when you propose. Just don't overdo it, as it will look fake.Girls love to be pampered so when you intend to propose, make sure you are chivalrous enough to make her feel special and at the same time not look vulnerable. Women want a strong man, not a wimp. You can even propose the girl at a place where you first met or a place you have fond memories of as these places have an association attached to it that will give less chance to her for refusal. Don't force her to say yes. The worst you can do is to push her around and threaten her dignity. Tell her to take her time and that you shall wait. Be a gentleman and respect her. Incase she doesn't want a relationship; she will let you know of it then and there.",
                    group1) { CreatedOn = "Group", CreatedTxt = "Ways & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Proposing a Girl", bgColour = "#6495ED", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/11.jpg")), CurrentStatus = "Valentine Proposal Ideas" });
            group1.Items.Add(new SampleDataItem("Group-1-Item-2",
                     "Proposing a Guy",
                     "Guys can be highly emotional and tend to feel vulnerable in overwhelming emotional situations. So make sure you both are in a place that is not too crowded. The idea of proposing your guy in the middle of a game is not very ideal! The right time for proposing also aids in making it a yes form your beloved.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nGuys can be highly emotional and tend to feel vulnerable in overwhelming emotional situations. So make sure you both are in a place that is not too crowded. The idea of proposing your guy in the middle of a game is not very ideal! The right time for proposing also aids in making it a yes form your beloved. It is not necessary to propose him on Valentine’s Day but make sure there are no other confusions around. Make it memorable. A guy loves to be wooed as much as girls do. Propose in a place that means a lot to him. Maybe a neighborhood park where he grew up chasing squirrels, or the place where you went for your first date, all are special to him and can be made memorable by proposing him there. In this age of emails and sms, love letters do have a magical effect on your beloved. It is a lot easier for girls who are shy or introvert or who feel that things are better expressed when written than said. Don’t use extremely flowery language as it will look like an essay, just be simple and lay your emotions on the paper. \n\nNot only girls, even guys love to receive flowers. So this Valentine's Day, send him a big bouquet of roses and card in which it is written, Will you marry me? A girl too can propose by recording a CD of the moments they have shared together with a common song of their choice and get it delivered at his home. You can even be there while he is watching it for his instant reaction. If you are bold enough and carry it well, get down on knees and propose him in a feminine style. It will surely take his heart as he will lot impressive and wooed. Don’t show desperation as you might end up loosing him. Put a ring in a glass of juice and amaze him with the proposal. He will be quite surprised with this gesture and will say yes. Radio has again come in the limelight these days and proposing your beloved live will be a sweet gesture, that will insist him on his own to say yes.",
                     group1) { CreatedOn = "Group", CreatedTxt = "Ways & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Proposing a Guy", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/12.jpg")), CurrentStatus = "Valentine Proposal Ideas" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-3",
                     "Right Time to Say I Love You",
                     "The first and foremost thing is to make sure if you are actually in love with the person or it is just a lust. Give your relation some time to grow. If you are proposing after a few weeks of meeting the person, chances are that it is just an infatuation.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nThe first and foremost thing is to make sure if you are actually in love with the person or it is just a lust. Give your relation some time to grow. If you are proposing after a few weeks of meeting the person, chances are that it is just an infatuation. Be a little patient before proposing and give a few months before going ga- ga over him. With time the novelty will surely wipe off and if you feel you can live with the whacky habits of him/ her, go forward. Always propose when you feel you are ready to take a person in your life. Do some introspection and come to a sane conclusion. Look for the non- verbal signals he is sending out to you like his caring nature, his interest in you and your life. Make sure the other person is interested in having a life-long relation with you. Keep dropping hints now and then to see the response and then move forward with it. Timing is another important factor before proposing your love. Make sure that you are not proposing them when he/ she is upset, is leaving for work, on a telephone call or via email. This will not reveal your true feelings and chances are that you end up with a rejection or no answer.\n\nDon’t just plan the exact time or setting for the same. As the feelings are genuine so should be your spontaneous proposal. So instead of planning out the perfect candle light dinner for her with romantic setting, say the magical words while you are hanging out as usual and in the middle of it, look for a moment when you can propose. Your heartfelt feeling while deep looking into the eyes will definitely take his/ her heart away. The proposal is just a formality. Make sure you talk about marriage on a much serious level to explore his thoughts before you actually pop up the proposal.Make sure your beloved is in a good mood. Else, the possible Yes may turn into a cold No.",
                     group1) { CreatedOn = "Group", CreatedTxt = "Ways & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Right Time to Say I Love You", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/13.jpg")), CurrentStatus = "Valentine Proposal Ideas" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-4",
                     "Top Ten Ideas for Romantic Proposal",
                     "Propose her through the old world charm by popping up with your question while dining or just when the sun is going down. The romantic environment will add to your emotions and it will seem that the world has conspired for two of you to meet. There are minimal chances of rejection.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nPropose her through the old world charm by popping up with your question while dining or just when the sun is going down. The romantic environment will add to your emotions and it will seem that the world has conspired for two of you to meet. There are minimal chances of rejection. Have your proposal aired on the radio on your beloved's favorite station. Make sure he/she is listening to the station at that time. You can even reach out to the RJ through their numbers aired and bowl him/her over. Record a personal home video of the moments you spent together. You can also include nicknames you use for each other. Get your friends to re-enact these moments. In the end, the video shows you, proposing your love to your cherished one. Get it delivered to your beloved through a friend along with a bouquet of roses. It sure to win your beloved's heart and there is no way he/ she rejects such a cute proposal. Another great idea for a proposal is to express your love a little ‘filmy way’ on the big screen. Make a plan for a movie and pre-plan with the manager of the theater to have your proposal aired on the screen just before the movie begins. The always-energetic crowd will be your confidence booster and is sure to get a positive reply for your proposal. Another great idea to propose is on your beloved's birthday. It will make that day so special that it will be etched in his/her memory forever. You can take him/her for dinner and propose when it is just the two of you, or you can do it in front of his/her family and friends.\n\nA unique idea is to gift your beloved a jigsaw puzzle. Make sure you take out one piece from the puzzle beforehand. Ask him/her to solve the puzzle. When he/she realizes one piece is missing, take that piece out from your pocket in which it is written, Marry me because you complete me. It is a beautiful way to show your beloved how much he/she means to you. Proposing while taking a walk in a romantic atmosphere like by the sea-shore, a secluded park or near a historical monument is also a good idea. It is simple, unexpected and is sure to win over your love. Make sure the timing is either early morning or late evening, around twilight. It adds to the romance. Gift him/her a small book in which Will you marry me is written in different languages. The english version can be at the end of the book. Another idea is to eliminate the page in which the english version is written. When they ask you what it means, propose! Why not take him/her to his/her dream destination and propose him/her there? It is surely going to be one of the best proposal ideas! The last and the best way to propose is the traditional way. Go down on one knee, hold a single red rose on which the ring is tied with a ribbon and pop the question. Though old, this way is a sure winner and works every time.",
                     group1) { CreatedOn = "Group", CreatedTxt = "Ways & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Top Ten Ideas for Romantic Proposal", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/14.jpg")), CurrentStatus = "Valentine Proposal Ideas" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-5",
                     "Wrong Ways of Proposing",
                     "Never propose over the phone or the Internet. It is the worst you can do, as it is too impersonal. Loving is all about expressing your feelings, which do not come out over the phone or the Internet. It shows you are not very serious about the whole thing. DO NOT propose in front of a large crowd.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\n Never propose over the phone or the Internet. It is the worst you can do, as it is too impersonal. Loving is all about expressing your feelings, which do not come out over the phone or the Internet. It shows you are not very serious about the whole thing. DO NOT propose in front of a large crowd. You wouldn't want to embarrass your beloved and then get embarrassed yourself if he/she says no.DO NOT be too humorous. It will seem that you are not serious about this proposal. Being too humorous while taking a serious decision spoils the mood and the ambience and blows up the whole thing. DO NOT forget your manners and etiquettes while proposing. Don't propose while eating or while the person is at work. Give respect and you will get respect. DO NOT propose when you don't mean it. Many people propose in the spur of the moment and then later regret their action. Do not give someone hopes and then smash it all at once. NEVER ever propose on a phone, sms, emails or any type of communication where you are not there. DON’T overuse the prop while you are proposing as it might get on nerves of the other person. Keep it simple and straightforward but do it romantically.\n\nIt is absolutely WRONG to ask your friend or her friend to convey the message on your behalf. It won’t get you a positive reply and you might end up getting a no for your answer. Hence express your feelings, yourselves. This goes for both guys and girls. DON’T mix drinking and proposing as will only make a mockery of you with an obvious no. Mixing drinking with every possible thing is equally bad. It is a BAD IDEA to propose right after you had a fight as it is the clear case of wrong timing. You might have taken it in a lighter manner but the emotion of anger, frustration and hurt must be running high in the other person.",
                     group1) { CreatedOn = "Group", CreatedTxt = "Ways & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Wrong Ways of Proposing", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/15.jpg")), CurrentStatus = "Valentine Proposal Ideas" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-6",
                     "Saying Sorry To A Girl",
                     "The first step is 'self acceptance. You must always keep in mind that no person is perfect and everyone is bound to make mistakes at one point or the other. Hence, try to forgive yourself as well as your partner without evaluating the situation critically.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nThe first step is 'self acceptance. You must always keep in mind that no person is perfect and everyone is bound to make mistakes at one point or the other. Hence, try to forgive yourself as well as your partner without evaluating the situation critically. Try to work on the areas or situations that led to the fight to avoid any further complications and conflicts. Analyze the situation. Don't force her to accept your apology if she has still not relaxed. It can only make the situation worse. Let her calm down and approach her when she is cool enough to talk again.Never ever apologize on the phone or the Internet. It is way too impersonal and shows how insensitive you are. If you have a problem facing her and talking, then write her a letter and deliver it yourself. Wait outside until she reads it fully. Accept your mistake and get done with it as soon as possible. Don't go over the situation and what caused the fight again and again.\n\nIf she asks for time, don't hesitate. Ask her what you can do to make her feel better and be genuine. Your feelings have to be heartfelt. Always remember, a woman can see through fake feelings. A bunch of flowers or a small gift can help a lot in making a woman feel better and cared of. And you have better chances of patching up again!",
                     group1) { CreatedOn = "Group", CreatedTxt = "Ways & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Saying Sorry To A Girl", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/16.jpg")), CurrentStatus = "Valentine Proposal Ideas" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-7",
                     "Saying Sorry To A Guy",
                     "Guys do not prefer sitting down immediately after a fight and talk about it. They are usually raging inside and you can get a taste of their bad attitude if you force them to listen to you. Leave your man alone to give him time to cool down. Do not bother him with phone calls or text messages.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nGuys do not prefer sitting down immediately after a fight and talk about it. They are usually raging inside and you can get a taste of their bad attitude if you force them to listen to you. Leave your man alone to give him time to cool down. Do not bother him with phone calls or text messages. Most of the times guys are ones who call up to talk things out. Wait for a day or two. Then go to their place and sort things out. Again, make sure he hasn't had a bad day at work or any other sort of thing that has him in low spirits.Try not to dig old issues and blame each other. Keep in mind that it is the future that counts and dwelling on old unpleasant memories never helps. Contrary to what some people think, guys like flowers too. It is absolutely cool to give your man flowers and make him good dinner to make him feel special. Always remember that inside every man there is a child that responds to love and affection. Make sure you pamper him and tell him how much you love him after you apologize. Spend some cozy moments together and let him know that he is special and means a lot to you.",
                     group1) { CreatedOn = "Group", CreatedTxt = "Ways & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Saying Sorry To A Guy", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/17.jpg")), CurrentStatus = "Valentine Proposal Ideas" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-8",
                     "Ways To Tell A Guy Likes You",
                     "He takes interest in your day-to-day life. He asks how your day was and feels concerned if anything is wrong. His behavior suddenly changes when you come around. He suddenly mellows down and becomes a bit quieter while chatting with his friends. Dead give away! He remembers almost every small thing that you say.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nHe takes interest in your day-to-day life. He asks how your day was and feels concerned if anything is wrong. His behavior suddenly changes when you come around. He suddenly mellows down and becomes a bit quieter while chatting with his friends. Dead give away! He remembers almost every small thing that you say. It shows how special you are. He calls up just to listen to your voice and gives a silly reason when you ask as to why he called. He insists on meeting up every now and then and admits how much he loves being around you. He is good with everyone, but with you he goes that extra mile to ensure comfort. His body language changes when you are there. He displays affection through subtle hints like stroking your cheek slightly, taking your hand in his palms and being less stiff. He tries to be overtly friendly with you and takes that extra step to know more about you, your family and your friends.\n\nWhen he praises only about you to his friends and family, it means he has got special feelings for you. Friends are a total give away. If his friends treat you in a special way or say something about the two of you being couples, you know he is in love. He looks in your eyes and you feel there is something special about it. He has this shine when he is looking at you. He keeps looking at you when you are around and shy’s away when you look at him directly. These are some sure signs that tell a guy really likes you. But do not confront him as soon as you find these signs in him. Give him some time to realize that he loves you. On the other hand, you can show him in your own ways that you like him too. You will know the right time to confide, you just do!",
                     group1) { CreatedOn = "Group", CreatedTxt = "Ways & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Ways To Tell A Guy Likes You", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/18.jpg")), CurrentStatus = "Valentine Proposal Ideas" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-9",
                     "Asking A Girl Out",
                     "Relax and look confident (even if you are not!). No woman likes a man who stutters and stammers while asking her out on a date. Your confidence will most certainly impress her. She will think that you are a man who is so sure of himself. Ask her casually for a game or lunch.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nRelax and look confident (even if you are not!). No woman likes a man who stutters and stammers while asking her out on a date. Your confidence will most certainly impress her. She will think that you are a man who is so sure of himself. Ask her casually for a game or lunch. Do not utter the word date if you know she is going to freak out. You can simply mention that you are going to your favorite restaurant for lunch, and ask her if she would like to join you. If she is skeptical about going out alone, mention that a couple of your friends are going too. She could bring her friends along if she wants to. It is very important for her to feel comfortable, and you should not mind some friendly disturbance for her convenience.\n\nBe specific about plans. Don't leave her clueless as to where are you going to take her. Mention the name of the restaurant or hang-out zone you have planned to take her to. This will make her comfortable with the idea. Do not push her if she is hesitant. You may blow off another chance that may be lurking in the corner. Patient wait as there is always a next chance. She will definitely say ‘yes’, once she has developed that confidence in you. Just that it may take some time! Do not hit on her directly. Don't ask, Would you like to go out with me? Any woman would be taken aback at this sudden remark and may back out, much to your dislike. Nothing is as bad as it looks. If she says ‘no’ this time, there is always a next time. So don't lose heart. Her ‘No’ shouldn't stop you from asking. You have to assure her that you are around and she can take her own sweet time. Strong decisions, as it as, are never taken impulsively.",
                     group1) { CreatedOn = "Group", CreatedTxt = "Ways & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Asking A Girl Out", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/19.jpg")), CurrentStatus = "Valentine Proposal Ideas" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-10",
                     "Asking A Guy Out",
                     "Put on a nice dress and apply a bit of make up to enhance your features. It is important to look good when you ask out. Your chances of hearing a yes sure become brighter! Don't hesitate to make the first move. The guy you want to ask out may be genuinely interested in you.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nPut on a nice dress and apply a bit of make up to enhance your features. It is important to look good when you ask out. Your chances of hearing a yes sure become brighter! Don't hesitate to make the first move. The guy you want to ask out may be genuinely interested in you. It could be that he is probably shy, or thinks that you are taken. If you are stuck in a scenario like this, there is no harm in being the first one to ask out. Don't ask him as if he is appearing for an interview. Just relax and let him relax as well. Strike a normal conversation and come to the point only when you think he is comfortable. Smile and make the atmosphere more relaxed and easy. Try to come across as genuine with your proposal. Matters of heart are to be truly heart-felt, or they just lose their meaning. It is certainly difficult to say ‘No’ to a sincere and earnest proposal. \n\nMost (if not all) guys prefer some adventure on date. So, if you are asking out, suggest something like hiking, kayaking, or a game of bowling. There will be hardly any guy on earth to say no to an exciting proposition like this. Make sure that you are willing to take up the adventure. If you have fixed a lunch or dinner, make sure you do any required arrangement for it. Since you asked him out, make sure you pay for it, unless he really insists. Don't forget to thank the man on accepting your proposal. It is perfect to let him know how grateful you are to him, but don’t be too sugar-coated with your words. Just be courteous and you will be respected for it. Lastly, if you receive a ‘No’ as the answer, don’t loose your heart. There is always a next chance, so, keep your hopes alive.  Continue to be friends with him and ask again when the time is right.",
                     group1) { CreatedOn = "Group", CreatedTxt = "Ways & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Asking A Guy Out", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/20.jpg")), CurrentStatus = "Valentine Proposal Ideas" });
					 
            this.AllGroups.Add(group1);


			
			
         
        }
    }
}
