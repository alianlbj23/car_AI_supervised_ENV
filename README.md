# car_AI supervsed learning Unity ENV
## 訓練用程式碼
https://github.com/alianlbj23/car_AI_RL
## 安裝
* 直接將裡面的東西用Unity開，可以用別的版本開啟(2022.3.9f1可以)
* 打開後左下有個Scenes，選class
* supervised 程式碼 : https://github.com/alianlbj23/car_AI_supervised_learning
## 重點程式碼
### 管理車體驅動、環境數值收送
* trainingManager
    * ROS的topic收送
    * 修改run model的名稱，run model為model inference用，後面隨便加個東西，變成蒐集資料
    * CarMove() : wasd控制和調驅動輪子數值
    * OnWebSocketMessage : 收到輪速後的動作(inference用)
    * Send() : 傳送資料
### 車體驅動
* Robot
    * 取得目前所有關於車體的資料
    * 傳送驅動數值給輪子
* MotionSensor
    * 取得車子座標、轉速、四位數轉角
* MotorMoveForward
    * 收到數值讓車輪轉動
### 定義要回傳給AI的資料
* State (不一定都要用，但沒定義一定不能用)
