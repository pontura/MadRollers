# Catoff Unity SDK

The **Catoff Unity SDK** is a Unity package that provides easy integration with the Catoff Challenge API. It allows Unity developers to create challenges, register players, update scores, and fetch leaderboards.

## 📌 Features
- Create a new challenge
- Register a player for a challenge
- Update player scores
- Fetch the leaderboard

## 📥 Installation

### Option 1: Install via Git URL
1. Open your Unity project.
2. Go to **Window** > **Package Manager**.
3. Click the **+** button and select **Add package from git URL...**
4. Enter the repository URL:
   ```
   https://github.com/CatoffGaming/catoff-unity-sdk.git
   ```
5. Click **Add**.

### Option 2: Install via Unity Package
1. Download the latest `.unitypackage` from the [Releases](https://github.com/CatoffGaming/catoff-unity-sdk/releases).
2. Open your Unity project.
3. Drag and drop the `.unitypackage` file into Unity.
4. Import all files.

## 🚀 Usage

### Initialize the SDK
```csharp
using CatoffSDK;

ChallengeSDK sdk = new ChallengeSDK("https://stagingapi2.catoff.xyz/", "YOUR_API_KEY");
```

### Create a Challenge
```csharp
CreateChallengeDto challenge = new CreateChallengeDto {
    ChallengeName = "Ultimate Race",
    ChallengeDescription = "Complete the race in record time",
    StartDate = 1695408000000,
    EndDate = 1696000000000,
    GameID = 10,
    Wager = 0.5,
    ChallengeCategory = "GAMING",
    Currency = "SOL",
    UserAddress = "0x1234567890abcdef"
};

string response = await sdk.CreateChallenge(challenge);
Debug.Log(response);
```

### Register a Player
```csharp
string response = await sdk.CreatePlayer(123, "0xPlayerWalletAddress");
Debug.Log(response);
```

### Update Player Score
```csharp
string response = await sdk.UpdatePlayerScore(123, 500, "0xPlayerWalletAddress");
Debug.Log(response);
```

### Fetch Leaderboard
```csharp
string leaderboardData = await sdk.GetLeaderboard(123);
Debug.Log(leaderboardData);
```

## 🛠 Requirements
- Unity 2021 or later
- .NET Standard 2.0+
- Newtonsoft.Json (included in Unity 2021+)

## 📄 License
This project is licensed under the MIT License.

## 🤝 Contributing
Feel free to submit pull requests or open issues in the [GitHub repository](https://github.com/CatoffGaming/catoff-unity-sdk/issues).