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

    SubmitScore: function(userId, score, username) {
        window.SubmitScore(userId, score, username);
    },

    GetHighScores: function() {
        window.GetHighScores();
    }
});