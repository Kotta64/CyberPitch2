# CyberPitch2


## CyberPitch2とは - About CyberPitch
CyberPitch2は[RoboCup Soccer Simulation 2D](https://ssim.robocup.org/)における競技観戦者向けの視覚化システムです。

CyberPitch2 is a visualization tool for competition spectators in [RoboCup Soccer Simulation 2D](https://ssim.robocup.org/).

![Image](https://github.com/user-attachments/assets/626e9ba8-e30c-47ec-af3d-3e32afeb1e84)

## 機能 - Functions
-   起動時に右クリックをすると出てくるウィンドウ
    -   Connect     → localhost:6000に接続 - Connect to localhost:6000
    -   Connect to  → Connect to any IP address and port number
    -   Open rcg    → Open rcg file
-   試合中の画面
    -   <img src="https://github.com/user-attachments/assets/3118d489-5be9-494b-abb8-4cdf62cef07a" width="25"> キックオフの指令を送信 - Send kick-off instruction
    -   <img src="https://github.com/user-attachments/assets/12a2ebba-bf50-4a5e-b4d5-08f8c389a078" width="25"> カメラの切り替えボタン - Camera switch button
    -   <img src="https://github.com/user-attachments/assets/551c5bff-8500-4ce6-bd15-ea1eb8207f76" width="25"> 設定画面を開くボタン - Open settings window
-   設定画面
    -   Mnimap        → 画面左下に表示されるミニマップのオンオフ - Minimap on/off displayed in the lower left corner of the screen
    -   ReplayVideo   → ゴール時のリプレイ映像のオンオフ - Replay video on/off at goal
    -   Volume        → 音量調整 - Volume control
    -   これらの設定は次回起動時にも適用される - These settings are also applied at the next startup.
    -   Reset Game    → 試合を中断し、初期画面へ遷移 - Stop the match and move to the initial screen.
-   以下のように、アプリケーション起動時にrcssserverのIPアドレスをコマンドライン引数として指定することでrcssserverへの接続処理を簡略化可能 <br>
    The connection process to rcssserver can be simplified by specifying the IP address of rcssserver as a command line argument when starting the application as follows
```
> ./CyberPitch2.x86_64 127.0.0.1
```

## 注意事項 - Precautions
-   WindowsにてCyberPitch2を使用する場合はファイアーウォールの設定が必要になる場合があります。 <br>
    Firewall settings may be required when using CyberPitch2 on Windows. 
-   rcssserverに接続して使用する場合、synch modeはoffにしてください。 <br>
    When connecting to rcssserver for use, synch mode should be off.
-   自身でCyberPitch2の改良・ビルドをおこなう際は後述するアセットを導入する必要があります。 <br>
    If you wish to improve or build CyberPitch2 yourself, you will need to install the assets described below.


## 開発環境 - Development Environment
- CPU : Ryzen7 7700X
- MEMORY : DDR5-5200 32GB
- GPU : Geforce RTX3070ti 8GB
- OS : Ubuntu 22.04 LTS
- Unity : 6000.0.32f1
- Editor : VS Code

## 使用したアセット - Unity Assets
- [Quick Outline](https://assetstore.unity.com/packages/tools/particles-effects/quick-outline-115488)
- [Skybox Series Free](https://assetstore.unity.com/packages/2d/textures-materials/sky/skybox-series-free-103633)
- [Low Polygon Soccer Ball](https://assetstore.unity.com/packages/3d/low-polygon-soccer-ball-84382)
- [UnityStandaloneFileBrowser](https://github.com/gkngkc/UnityStandaloneFileBrowser)
