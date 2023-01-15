# LowPolyRPG
Unity3D를 이용한 쿼터뷰 형식의 RPG 게임 개인 프로젝트

## 개발 환경
- Unity 3D(2021.3.2f1)
- Visual Studio 2022
- C#

## 시작 화면
비동기 씬 전환(Load Scene Async)을 통한 프로그램 중지 및 지연 방지

**[구동 화면]**

![LoadSceneAsync](https://user-images.githubusercontent.com/80217301/211596011-16f210e4-0e3b-4c75-87f2-f089b7d6cbe9.gif)

 
## 플레이어
NavMeshAgent와 간단한 상태 패턴을 통해 플레이어 조작을 구현함

**[상태 패턴 알고리즘]**

![StateManager drawio](https://user-images.githubusercontent.com/80217301/211600927-6c90d740-3f15-47f0-9b52-576b0ae9d293.png)

**[구동 화면]**

![PlayerControl](https://user-images.githubusercontent.com/80217301/211601948-72593636-6cf0-4e16-b189-6abe67fb959c.gif)


## 스킬
스킬 데이터는 Scriptable Object로 만들어 관리했으며 스킬 스크립트는 Prefab에 담았다.

각 스킬 키에 해당하는 키를 누를 경우 SkillManager에서 스킬이 준비가 됐는지, 적이 근처에 있는지 등 조건을 탐색한 후 만족하면 스킬을 실행한다.

**[구동 화면]**

![Skill](https://user-images.githubusercontent.com/80217301/212345836-f4a44629-24a0-42a3-9e65-176bb70643be.gif)

## NPC 대화 & 퀘스트
퀘스트, NPC 데이터는 Scriptable Object로 만들어 관리했고 스크립트에서 해당 데이터를 가져와 표시해주기만 하면 된다.

퀘스트는 대화, 사냥 퀘스트 두 가지로 구현했다.

**[퀘스트 구상도]**

<img src = "https://user-images.githubusercontent.com/80217301/212541796-73da36ec-cc66-499a-844d-409c2e1a67ac.png"
width = 300px>

**[퀘스트  흐름도]**

<img src = "https://user-images.githubusercontent.com/80217301/212541823-5408237a-9902-4097-91e6-d5f0a3175266.png"
width = 300px>

**[구동 화면]**

![NPC](https://user-images.githubusercontent.com/80217301/212357760-42430250-d4ae-48e7-8541-e29db45a1935.gif)

## 컷신
Unity의 Timeline 시스템을 이용해 컷신을 제작했으며 카메라 무빙을 위한 커스텀 Playable을 제작했다.

컷신을 위한 Event Camera는 평소에는 비활성화돼있다.

컷신이 시작되면 Event Camera가 활성화되고 Main Camera가 Event Camera의 Transform을 따라가게 하였다.

**[커스텀 Playble Track]**

![image](https://user-images.githubusercontent.com/80217301/212466062-20b67b6c-d536-4d1f-8c0a-4beadcf3067a.png)

**[구동 화면]**

![Timeline](https://user-images.githubusercontent.com/80217301/212466142-2c8d4224-5475-4145-9208-0e48e2a79953.gif)
