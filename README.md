# 🎮3D RPG 게임 프로젝트 - EscapeDungeon
- 유니티 버전 : 2022.3.8f1
<br>

## 개요
- 기획 의도 : 수업에서는 TPS같은 shooting 게임 위주로 배워서 내가 좋아하는 게임 장르를 직접 만들어볼 기회가 없었기 때문에 이번 프로젝트
를 기회로 간단한 RPG게임을 만들어 보고 싶었다.
- 게임 시나리오 : 던전 내에 있는 몬스터들을 물리치고 필드 내에 있는 더 강한 무기를 획득해서 보스 몬스터를 물리치고 던전을 탈출하여 게임을
클리어한다.

<br>

## 프로젝트 조직도
<div align="center">
<img src="https://github.com/user-attachments/assets/47c4d11d-0193-47e0-94a6-050120f42200" alt="Image" width="600" />
</div>

<br>

## 게임 설명
|<img src="https://github.com/user-attachments/assets/802be4d7-4e1c-4abe-9249-520d1d19fd95" alt="Image" width="300" />|<img src="https://github.com/user-attachments/assets/ef582a5c-8b76-4558-8c0e-d5eeae350293" alt="Image" width="300" />|
|:-------------:|:---------:|
| 타이틀 화면 | 시작 후 |
- 타이틀 화면에서 GameStart버튼 클릭 후 모든 요소가 활성화되며 게임이 시작됩니다.

<br>

## 게임 플레이 방식
- 캐릭터 이동 방법

|이동방향|상|하|좌|우|
|:--------:|:--:|:-:|:---:|:---:|
|키보드|W|A|S|D|
|방향키|⬆️|⬅️|⬇️|➡️|

- 인게임 UI

|플레이어HP|QuickInventory|BossHP|
|:-------:|:------:|:----:|
|<img src="https://github.com/user-attachments/assets/2f5382f8-1851-411b-9306-dad7be8bd6ad" alt="Image" width="300" />|<img src="https://github.com/user-attachments/assets/8171a298-c992-412c-8d2f-4c7d7fed57e6" alt="Image" width="300" />|<img src="https://github.com/user-attachments/assets/e06a3a1e-fd86-4f16-907b-5bf08775e9d5" alt="Image" width="300" />|
|플레이어 체력바|무기 인벤토리, 표시된 키 입력시 무기 교체|보스 체력바|

- 일반 몬스터 및 보스 몬스터

|일반 몬스터|보스몬스터 패턴1|보스몬스터 패턴2|
|:-------:|:------:|:----:|
|<img src="https://github.com/user-attachments/assets/dbc955cb-a8f2-4fc0-9d69-69d6ae1f8a53" alt="Image" width="300" />|<img src="https://github.com/user-attachments/assets/4542a288-bedb-4452-84e8-4be3ab6ad8e3" alt="Image" width="300" />|<img src="https://github.com/user-attachments/assets/7c501b74-92b1-4dca-b994-49fe0027e4d3" alt="Image" width="300" />|
|정해진 스폰 포인트에서 계속 생성|내비게이션을 사용해 플레이어를 끝까지 쫓아감|바위가 굴러갈수록 점점 크기와 데미지가 커진다|
