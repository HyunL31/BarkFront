## 🎮 멍멍전선:2077 - 2D 디펜스 게임

---

## 📂 담당 파트

- 타이틀, 오프닝 및 엔딩 씬 구현
- 타이밍 바 미니게임, CCTV 로봇 시스템 구현
- 낮 파트 자원 시스템, 타이머 시스템 구현
- 게임 전체 통합 및 폴리싱 작업

---

## 🧭 **컨셉 개요**

- **제목:** 멍멍전선:2077
- **장르:** 2D 자원 관리 (미니게임) + 디펜스 게임
- **플랫폼:** PC
- **핵심 아이디어**
    - 낮에는 맵을 돌아다니며 상자를 열어 랜덤으로 자원을 획득하고 밤에는 로봇을 막는 디펜스 게임
    - AI 로봇에게 익숙하지 않은 로스트 테크놀로지 ‘키보드’를 통해 강아지를 소환한다.
- **스토리:** AI 로봇이 세상을 점령한 디스토피아 세계관, 감정을 잘 이해하는 강아지와 인간만이 살아남아 반란을 일으키고자 한다.

---

## 🧱 개발 내용

| 구현 | 담당 파트 설명 |
| --- | --- |
| Opening, Ending | Swap을 사용하여 컷씬 배경 교체, Coroutine으로 자막 재생 및 타이핑 효과 구현, 게임 오버에 따른 엔딩 분기 구현 |
| 자원 시스템 | 싱글톤 사용, 랜덤 확률 설정, enum으로 자원 종류 관리 |
| 타이머 시스템 | 싱글톤으로 낮 밤 연계 |
| 타이밍 바 미니게임 | 타겟의 Rect Transform 기준으로 승패 판정, 3단계로 구현 |
| CCTV 감시 로봇 미니게임 | 낮 동안 랜덤으로 이벤트 발생, 10초 안에 도망 버튼 클릭, CCTV UI 범위 안에 플레이어가 들어오면 실패, 실패 시 캐릭터 캡쳐 후 -10 코인 |
| 랜덤 이벤트 | Action 기반 콜백 구조로 구현 |

---

## 🎥 게임 플레이

### 1️⃣ CutScene

![image.png](attachment:fa4d678c-0042-4ada-9d1a-c96b799bfeb0:image.png)

![image.png](attachment:e4962e19-b9c2-4cd2-aa4d-c8a70b55159b:image.png)

### 2️⃣ 타이밍 바 게임

![image.png](attachment:05336b2a-2ffc-4e6a-a933-3c1aeb1c199e:image.png)

![image.png](attachment:7a20354e-aef0-4e08-9a77-9552e3b15c9e:image.png)

### 3️⃣ CCTV 감시 로봇 게임

![image.png](attachment:eeea521f-97ff-4b74-b3b8-99594d2cc0b2:image.png)

![image.png](attachment:809c9e74-e86a-4050-9a40-33c9dadcb5a2:image.png)

---

## 📁 **사용한 에셋**

- 맵, 로봇, 강아지 - [itch.io](http://itch.io) asset store
- UI - Figma로 제작
- 컷씬 배경 - chatGPT로 생성
