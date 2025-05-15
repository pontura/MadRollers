mergeInto(LibraryManager.library, {
    SignIn: function() {
        return new Promise(function(resolve, reject) {
            window.SignIn().then(function(userId) {
                resolve(userId);
            }).catch(function(error) {
                reject(error);
            });
        });
    },

    SubmitScore: function(usernamePtr, score) {
		var username = UTF8ToString(usernamePtr);
        window.SubmitScore(username, score);
    },

    GetHighScores: function() {
        window.GetHighScores();
    }
});