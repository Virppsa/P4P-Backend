namespace P4P.Services.EmailService;

// 4. Delegates usage;
// 3. Generics (in delegates, events and methods)(at least two)
public delegate void EmailEventHandler<TEventArgs>(UserService sender, TEventArgs args);