mergeInto(LibraryManager.library, {
    SignInAnonymously: function() {
        return new Promise(function(resolve, reject) {
            window.SignInAnonymously().then(function(userId) {
                resolve(userId);
            }).catch(function(error) {
                reject(error);
            });
        });
    },

    SubmitScore: function(userId, score, username) {
        window.SubmitScore(userId, score, username);
    },

    GetHighScores: function() {
        window.GetHighScores();
    }
});