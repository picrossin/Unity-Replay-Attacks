# Performing and Preventing Replay Attacks in Unity
### by Jaden Goter and Camden Obertop
Replay attacks are where a malicious actor sniffs a packet or set of packets between two hosts and sends it again to achieve some malicious goal, such as giving themselves twice as many coins. These attacks can happen to multiplayer Unity games. We demonstrate how to perform one with Unity, the Mirror netoworking package, and a simple custom XAMPP PHP web server. We also delve into a secure version and speak of common prevention techniques.

![image](https://user-images.githubusercontent.com/43327093/167699337-8d377b28-b092-4a39-8d35-a6d9e589ec96.png)

## The Game and Implementation
We modified [Mirror's Pong example](https://mirror-networking.gitbook.io/docs/examples/pong) to include scoring. The out-of-the-box solution simply includes a ball bouncing back and forth and two paddles that you can move. We added a score that is kept track of in [`Scores.cs`](https://github.com/picrossin/Unity-Replay-Attacks/blob/main/Assets/Pong/Scripts/Scores.cs). We implemented two versions of keeping track of scoring: an insecure one that utilizes the XAMPP localhost web server with a [simple PHP API](https://github.com/picrossin/Unity-Replay-Attacks/blob/main/pong_web.php), and a secure one that uses the [`NetworkManagerPong.cs`](https://github.com/picrossin/Unity-Replay-Attacks/blob/main/Assets/Pong/Scripts/NetworkManagerPong.cs) and the [KCP Transport layer script](https://mirror-networking.gitbook.io/docs/transports/kcp-transport) to send a network message whenever a player scored. 

## Findings
We found that we had to implement a deliberately insecure scoring system via the XAMPP PHP API to even expose the packets enough to replay. Mirror's default KCP Transport layer encrypts UDP packets and does some end-to-end error checking to make replaying its packets impossible. Here is an example of one of the the Pong game's packets running over a LAN server in Wireshark:

![image](https://user-images.githubusercontent.com/43327093/167703430-795ddcfc-a45e-42a5-8a2b-3faa2cb70d95.png)

We tried replaying these packets with [Colasoft's Packet Player](https://www.colasoft.com/packet_player/), but had no luck with the secure implementation. Once we hooked up our localhost webserver though, we were able to observe the unencrypted HTTP packets in Fiddler:

![image](https://user-images.githubusercontent.com/43327093/167703670-28a23aa0-a354-4f69-9fce-47629b3f85db.png)

Here, we were able to replay the "scoring" packets and have the score increment on the server accordingly and score 100 points when we shouldn't have. 

![image](https://user-images.githubusercontent.com/43327093/167703991-02d5fec2-a25c-446a-8eea-d53c0fe90301.png)

Fiddler modifies the packets so that the server interprets them as new API POST requests coming in. It was impossible to spoof the UDP packets that Mirror sent.

Therefore, one should try to implement all of their multiplayer systems in packages like Mirror or Netcode for Gameobjects. They already filter out replay attacks with their robust transport layers. Implementing a multiplayer system could lead to problems, but if you do so, try to use timestamps, session IDs, or nonces to prevent replay attacks from happening, espcially if you are unable to encrypt your packets.
