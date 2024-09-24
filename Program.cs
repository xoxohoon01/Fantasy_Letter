using Newtonsoft.Json;

namespace Fantasy_Letter
{
    internal class Program
    {
        static int Screen_Width = 96;
        static int Screen_Height = 23;
        static float GameSpeed = 1.0f;

        static void Main()
        {
            Console.Title = "판타지 레터스";
            Console.CursorVisible = false;
            Player player = new Player();

            if (File.Exists("./data.json"))
            {
                Player json = JsonConvert.DeserializeObject<Player>(File.ReadAllText("./data.json"));
                player = json;
            }
            else
            {
                SaveData(ref player);
            }

            while (true)
            {
                GameStart(ref player);
            }
        }

        static void GameStart(ref Player _player)
        {
            Msg("☆  판타지 레터스 ☆", 0, -1, true, true);
            Msg("시작하려면", 0, 0,  true, false);
            Msg("아무키나 입력하세요.", 0, 1, false, false);

            CheckName(ref _player);
            CheckClass(ref _player);
            EnterField(ref _player);
        }

        static void CheckName(ref Player _player)
        {
            bool isFirst = true;
            while(true)
            {
                if (_player.name == "")
                {
                    DrawBox();

                    if (isFirst)
                    {
                        isFirst = false;
                        Msg("반갑습니다.", 0, -1, true, true);
                        Msg("이 곳은 시작의 섬...", 0, 0, true, false);
                        Msg("당신의 이름은 무엇입니까?", 0, 1, true, false);
                    }
                    else
                    {
                        Msg("당신의 이름은 무엇입니까?", 0, 0, true, true);
                    }
                    
                    _player.name = Question("이름: ");
                    if (_player.name != "")
                    {
                        break;
                    }
                    else
                    {
                        DrawBox();

                        Msg("당신의 이름이 궁금합니다...", 0, 0, true, true);
                        Msg("제게 이름을 말해주지 않겠어요?", 0, 1, false, false);
                        continue;
                    }
                }
                else
                {
                    break;
                }
            }
        }

        static void CheckClass(ref Player _player)
        {
            bool isFirst = true;
            while(true)
            {
                if (_player.classType == ClassType.None)
                {
                    if (isFirst)
                    {
                        isFirst = false;

                        DrawBox();
                        Msg($"{_player.name}..", 0, 0, true);
                        Msg($"당신의 모험이 궁금해지네요...", 0, 1, false);
                    }

                    DrawBox();
                    Msg("직업을 선택하세요.", 0, -2, true, true);
                    Msg("1. 전사", 0, -1, true, false);
                    Msg("2. 궁수", 0, 0, true, false);
                    Msg("3. 도적", 0, 1, true, false);
                    Msg("4. 마법사", 0, 2, true, false);
                    string choice = Question("직업: ");
                    if (choice != "")
                    {
                        try
                        {
                            int number = int.Parse(choice);
                        }
                        catch (FormatException)
                        {
                            choice = "0";
                        }
                        finally
                        {
                            int number = int.Parse(choice);
                            if (number == 1)
                            {
                                _player.classType = ClassType.Warrior;
                                _player.className = "전사";
                                _player.classNameModifier = "가";
                            }
                            else if (number == 2)
                            {
                                _player.classType = ClassType.Archer;
                                _player.className = "궁수";
                                _player.classNameModifier = "가";
                            }
                            else if (number == 3)
                            {
                                _player.classType = ClassType.Rogue;
                                _player.className = "도적";
                                _player.classNameModifier = "이";
                            }
                            else if (number == 4)
                            {
                                _player.classType = ClassType.Mage;
                                _player.className = "마법사";
                                _player.classNameModifier = "가";
                            }
                            else
                            {
                                DrawBox();
                                Msg("직업을 다시 선택해주세요.", 0, 0, true, true);
                                Msg("(숫자를 입력하세요.)", 0, 1, false, false);
                            }
                        }
                    }
                    else
                    {
                        DrawBox();
                        Msg("직업을 다시 선택해주세요.", 0, 0, true, true);
                        Msg("(숫자를 입력하세요.)", 0, 1, false, false);
                        continue;
                    }

                    if (_player.classType != ClassType.None)
                    {
                        switch (_player.classType)
                        {
                            case ClassType.Warrior:
                                Msg("전사.", 0, -1, true, true);
                                Msg("적들의 공격에도 끄떡없는 강인함으로", 0, 0, true, false);
                                Msg("전투를 이끌어 나갑니다.", 0, 1, false, false);
                                break;
                            case ClassType.Archer:
                                Msg("궁수.", 0, -1, true, true);
                                Msg("빠른 공격을 이어가며", 0, 0, true, false);
                                Msg("적에게 공격할 기회조차 주지 않습니다.", 0, 1, false, false);
                                break;
                            case ClassType.Rogue:
                                Msg("도적.", 0, -1, true, true);
                                Msg("적의 틈새를 파고들어 역린을 노려", 0, 0, true, false);
                                Msg("치명상을 입히는 능력이 탁월합니다.", 0, 1, false, false);
                                break;
                            case ClassType.Mage:
                                Msg("마법사.", 0, -1, true, true);
                                Msg("강력한 주문을 외워 마법을 사용하지만", 0, 0, true, false);
                                Msg("어쩌면 재앙을 불러올지도 모릅니다.", 0, 1, false, false);
                                break;
                        }

                        Msg("당신의 새로운 여정에 가호가 함께하기를..", 0, 0, false, true);
                        SetStatus(ref _player);
                        break;
                    }
                    else continue;
                }
                else
                {
                    break;
                }
            }
        }

        static void SetStatus(ref Player _player)
        {
            int damage = 0;
            int damageRange = 0;
            int armor = 0;
            int hp = 0;
            int accuracy = 0;
            int dodge = 0;

            int damage_perLevel = 0;
            int damageRange_perLevel = 0;
            int armor_perLevel = 0;
            int hp_perLevel = 0;
            int accuracy_perLevel = 0;
            int dodge_perLevel = 0;

            switch (_player.classType)
            {
                case ClassType.None:
                    break;
                case ClassType.Warrior:
                    damage = 15;
                    damageRange = 2;
                    armor = 5;
                    hp = 100;
                    accuracy = 7;
                    dodge = 0;

                    damage_perLevel = 7;
                    damageRange_perLevel = 3;
                    armor_perLevel = 4;
                    hp_perLevel = 80;
                    accuracy_perLevel = 1;
                    dodge_perLevel = 1;
                    break;

                case ClassType.Archer:
                    damage = 15;
                    damageRange = 3;
                    armor = 2;
                    hp = 100;
                    accuracy = 6;
                    dodge = 0;

                    damage_perLevel = 7;
                    damageRange_perLevel = 5;
                    armor_perLevel = 2;
                    hp_perLevel = 50;
                    accuracy_perLevel = 1;
                    dodge_perLevel = 1;
                    break;

                case ClassType.Rogue:
                    damage = 15;
                    damageRange = 1;
                    armor = 3;
                    hp = 100;
                    accuracy = 8;
                    dodge = 1;

                    damage_perLevel = 9;
                    damageRange_perLevel = 1;
                    armor_perLevel = 3;
                    hp_perLevel = 60;
                    accuracy_perLevel = 1;
                    dodge_perLevel = 1;
                    break;

                case ClassType.Mage:
                    damage = 15;
                    damageRange = 5;
                    armor = 1;
                    hp = 100;
                    accuracy = 6;
                    dodge = 0;

                    damage_perLevel = 7;
                    damageRange_perLevel = 7;
                    armor_perLevel = 1;
                    hp_perLevel = 40;
                    accuracy_perLevel = 1;
                    dodge_perLevel = 1;
                    break;
            }

            _player.itemDamage = _player.inventory.weapon.damage + _player.inventory.head.damage + _player.inventory.top.damage + _player.inventory.bottom.damage;
            _player.itemDamageRange = _player.inventory.weapon.damageRange + _player.inventory.head.damageRange + _player.inventory.top.damageRange + _player.inventory.bottom.damageRange;
            _player.itemArmor = _player.inventory.weapon.armor + _player.inventory.head.armor + _player.inventory.top.armor + _player.inventory.bottom.armor;
            _player.itemHp = _player.inventory.weapon.hp + _player.inventory.head.hp + _player.inventory.top.hp + _player.inventory.bottom.hp;
            _player.itemAccuracy = _player.inventory.weapon.accuracy + _player.inventory.head.accuracy + _player.inventory.top.accuracy + _player.inventory.bottom.accuracy;
            _player.itemDodge = _player.inventory.weapon.dodge + _player.inventory.head.dodge + _player.inventory.top.dodge + _player.inventory.bottom.dodge;

            if (_player.level == 0) { _player.level = 1; _player.maxExp = 100; }
            _player.damage = damage + (damage_perLevel * (_player.level - 1));
            _player.damageRange = damageRange + (damageRange_perLevel * (_player.level - 1));
            _player.armor = armor + (armor_perLevel * (_player.level - 1));
            _player.hp = hp + (hp_perLevel * (_player.level - 1));
            _player.accuracy = accuracy + (accuracy_perLevel * (_player.level - 1));
            _player.dodge = dodge + (dodge_perLevel * (_player.level - 1));

            if (!_player.isCompleteTutorial) EnterTutorial(ref _player);
        }

        static void EnterTutorial(ref Player _player)
        {
            _player.isCompleteTutorial = true;

            Msg("", 0, 0, false, true);
            Msg("...", 0, 0, false, true);
            Msg("*...이보게.*", 0, 0, false, true);
            Msg("...", 0, 0, false, true);
            Msg("*이제 그만 일어나게나.*", 0, 0, false, true);
            Msg("???: 반갑네.", 0, 0, false, true);

            Msg("???: 나는 이 섬의 촌장.", 0, 0, false, true);

            Msg("이태훈: 이태훈이라네...", 0, 0, false, true);

            Msg($"이태훈: {_player.name}..", 0, 0, true, true);
            Msg($"자네는 풋내기 {_player.className}{(_player.classNameModifier == "이" ? "이" : "")}로군.", 0, 1, false, false);

            Msg($"이태훈: 자네같은 풋내기는", 0, 0, true, true);
            Msg($"참으로 오랜만이야.", 0, 1, false, false);

            switch(_player.classType)
            {
                case ClassType.Warrior:
                    GetItem(ref _player, new Item($"초심자의 검", "무기", "약하다.", 0, 8, 2));
                    break;
                case ClassType.Archer:
                    GetItem(ref _player, new Item($"초심자의 활", "무기", "약하다.", 0, 8, 3));
                    break;
                case ClassType.Rogue:
                    GetItem(ref _player, new Item($"초심자의 단검", "무기", "약하다.", 0, 10, 1));
                    break;
                case ClassType.Mage:
                    GetItem(ref _player, new Item($"초심자의 지팡이", "무기", "약하다.", 0, 9, 5));
                    break;
            }
            _player.inventory.gold += 300;
            Msg("이태훈: 도움이 될만한 아이템들을", 0, 0, true, true);
            Msg("그대의 소매에 몰래 넣어놨다네.", 0, 1, false, false);

            Msg("이태훈: 여행을 떠나기 전에", 0, 0, true, true);
            Msg("장비를 한번 확인해보게나.", 0, 1, false, false);

            Msg("이태훈: 그대에게 놀라운 모험이 있기를..", 0, 0, false, true);
            EnterField(ref _player);
        }

        static void EnterField(ref Player _player)
        {
            int nowAction = 1;
            string CheckAction(int number)
            {
                if (nowAction == number)
                {
                    return "<";
                }
                else
                {
                    return "";
                }
            }

            while (_player.name != "")
            {
                SaveData(ref _player);
                DrawBox();
                Msg($"스테이터스 {CheckAction(1)}", 0, -1, true, true);
                Msg($"장비 {CheckAction(2)}", 0, 0, true, false);
                Msg($"상점 {CheckAction(3)}", 0, 1, true, false);
                Msg($"던전 {CheckAction(4)}", 0, 2, true, false);

                MsgOutBox("방향키와 엔터를 이용해 행동을 선택하세요.");

                ConsoleKey key = Console.ReadKey().Key;
                if (key == ConsoleKey.UpArrow) { nowAction = (int)MathF.Max(nowAction - 1, 1); }
                else if (key == ConsoleKey.DownArrow) { nowAction = (int)MathF.Min(nowAction + 1, 4); }
                else if (key == ConsoleKey.Enter)
                {
                    switch(nowAction)
                    {
                        default:break;
                        case 1:
                            EnterStatus(ref _player);
                            break;
                        case 2:
                            EnterInventory(ref _player);
                            break;
                        case 3:
                            EnterShop(ref _player);
                            break;
                        case 4:
                            EnterDungeon(ref _player);
                            break;
                    }
                }
            }
        }

        static void EnterStatus(ref Player _player)
        {
            int nowAction = 0;
            string CheckAction(int number)
            {
                if (nowAction == number)
                {
                    return "<";
                }
                else
                {
                    return "";
                }
            }

            while (_player.name != "")
            {
                DrawBox();
                Msg($"[뒤로가기] {CheckAction(0)}", 0, -6, true, true);
                Msg($"[ ※ 데이터삭제 ※ ] {CheckAction(1)}", 0, -5, true, false);

                Msg($"[{ _player.className}] {_player.name}", 0, -3, true, false);
                Msg($" Lv. {_player.level} ({_player.exp}/{_player.maxExp})", 0, -2, true, false);
                Msg($"보유골드: {_player.inventory.gold}", 0, -1, true, false);
                Msg($"공격력 {_player.damage} (+{_player.itemDamage})", 0, 1, true, false);
                Msg($"기량 {_player.damageRange} (+{_player.itemDamageRange})", 0, 2, true, false);
                Msg($"방어력 {_player.armor} (+{_player.itemArmor})", 0, 3, true, false);
                Msg($"체력 {_player.hp} (+{_player.itemHp})", 0, 4, true, false);
                Msg($"명중률 {_player.accuracy} (+{_player.itemAccuracy})", 0, 5, true, false);
                Msg($"회피력 {_player.dodge} (+{_player.itemDodge})", 0, 6, true, false);


                MsgOutBox("방향키와 엔터를 이용해 행동을 선택하세요.");

                ConsoleKey key = Console.ReadKey().Key;
                if (key == ConsoleKey.UpArrow) { nowAction = (int)MathF.Max(nowAction - 1, 0); }
                else if (key == ConsoleKey.DownArrow) { nowAction = (int)MathF.Min(nowAction + 1, 1); }
                else if (key == ConsoleKey.Enter)
                {
                    switch (nowAction)
                    {
                        default:
                            return;
                        case 0:
                            return;
                        case 1:
                            DeleteData(ref _player);
                            break;
                    }
                }
            }
        }

        static void EnterInventory(ref Player _player)
        {
            int nowAction = 0;
            string CheckAction(int number)
            {
                if (nowAction == number)
                {
                    return "<";
                }
                else
                {
                    return "";
                }
            }

            while (true)
            {
                Msg($"[뒤로가기] {CheckAction(0)}", 0, -2, true, true);
                Msg($"장비 확인 {CheckAction(1)}", 0, 0, true, false);
                Msg($"아이템 장착 {CheckAction(2)}", 0, 1, true, false);

                MsgOutBox("방향키와 엔터를 이용해 행동을 선택하세요.");

                ConsoleKey key = Console.ReadKey().Key;
                if (key == ConsoleKey.UpArrow) { nowAction = (int)MathF.Max(nowAction - 1, 0); }
                else if (key == ConsoleKey.DownArrow) { nowAction = (int)MathF.Min(nowAction + 1, 2); }
                else if (key == ConsoleKey.Enter)
                {
                    switch (nowAction)
                    {
                        default:
                            return;
                        case 0:
                            return;
                        case 1:
                            EnterEquipment(ref _player);
                            break;
                        case 2:
                            EnterChangeEquipment(ref _player);
                            break;
                    }
                }
            }
        }

        static void EnterEquipment(ref Player _player)
        {
            DrawBox();

            Msg("현재 장착중인 장비", 0, -2, true, true);
            Msg($"무기: {_player.inventory.weapon.name}", 0, -1, true, false);
            Msg($"머리: {_player.inventory.head.name}", 0, 0, true, false);
            Msg($"상의: {_player.inventory.top.name}", 0, 1, true, false);
            Msg($"하의: {_player.inventory.bottom.name}", 0, 2, false, false);
        }

        static void EnterChangeEquipment(ref Player _player)
        {
            int nowPage = 0;
            int nowAction = 0;
            string CheckAction(int number)
            {
                if (nowAction == number)
                {
                    return "<";
                }
                else
                {
                    return "";
                }
            }

            while (true)
            {
                bool hasPrevPage = false;
                bool hasNextPage = false;
                Msg($"[뒤로가기] {CheckAction(0)}", 0, -8, true, true);

                if (_player.inventory.items.Count < 1) nowAction = 0;

                //보유 아이템 5개 (페이지당)
                for (int i = 0; i < 5; i++)
                {
                    if (_player.inventory.items.Count > (nowPage * 5) + i)
                    {
                        Item nowItem = _player.inventory.items[(nowPage * 5) + i];
                        Msg($"{(nowPage * 5) + i + 1}. {nowItem.name} | {nowItem.type} | 공격력: {nowItem.damage} | 기량: {nowItem.damageRange} | 방어력: {nowItem.armor} | 체력: {nowItem.hp} | 금액: {nowItem.gold} {(nowItem.isEquip ? "(E)" : "")} {CheckAction(i + 1)}", 0, -6 + (i * 3), true, false);
                    }
                }

                if (nowPage > 0)
                {
                    hasPrevPage = true;
                    Msg($"[이전 페이지] {CheckAction(6)}", 0, 5, true, false);
                }
                if (_player.inventory.items.Count > (nowPage * 5) + 5)
                {
                    hasNextPage = true;
                    Msg($"[다음 페이지] {CheckAction(7)}", 0, 6, true, false);
                }
                MsgOutBox("방향키와 엔터를 이용해 행동을 선택하세요.");

                ConsoleKey key = Console.ReadKey().Key;
                if (key == ConsoleKey.UpArrow)
                {
                    if (nowAction == 6)
                    {
                        int count = 5;
                        while (_player.inventory.items.Count < (nowPage * 5) + count)
                        {
                            count--;
                        }
                        nowAction = count;
                    }
                    else if (nowAction == 7)
                    {
                        if (hasPrevPage) nowAction = 6;
                        else
                        {
                            int count = 5;
                            while (_player.inventory.items.Count < (nowPage * 5) + count)
                            {
                                count--;
                            }
                            nowAction = count;
                        }
                    }
                    else nowAction = (int)MathF.Max(nowAction - 1, 0);
                }
                else if (key == ConsoleKey.DownArrow)
                {
                    //nowAction이 아이템을 가리킬 때
                    if (nowAction >= 1 && nowAction <= 5)
                    {
                        //다음 아이템이 있을 때
                        if (_player.inventory.items.Count > (nowPage * 5) + nowAction && nowAction < 5)
                        {
                            nowAction = (int)MathF.Min(nowAction + 1, 7);
                        }
                        //다음 아이템이 없을 때
                        else
                        {
                            if (hasPrevPage)
                            {
                                nowAction = 6;
                            }
                            else if (hasNextPage)
                            {
                                nowAction = 7;
                            }
                        }
                    }
                    //nowAction이 0, 6일 때
                    else
                    {
                        if (nowAction == 0)
                        {
                            if (_player.inventory.items.Count > (nowPage * 5) + nowAction && nowAction < 5)
                            {
                                nowAction = (int)MathF.Min(nowAction + 1, 7);
                            }
                            else
                            {
                                if (hasPrevPage)
                                {
                                    nowAction = 6;
                                }
                                else if (hasNextPage)
                                {
                                    nowAction = 7;
                                }
                            }
                        }
                        else if (nowAction == 6)
                        {
                            if (hasNextPage)
                            {
                                nowAction = 7;
                            }
                        }
                    }
                }
                else if (key == ConsoleKey.Enter)
                {
                    if (nowAction >= 1 && nowAction <= 5)
                    {
                        switch (_player.inventory.items[nowPage * 5 + nowAction - 1].type)
                        {
                            case "무기":
                                _player.inventory.weapon.isEquip = false;
                                _player.inventory.weapon = _player.inventory.items[nowPage * 5 + nowAction - 1];
                                _player.inventory.weapon.isEquip = true;
                                break;
                            case "머리":
                                _player.inventory.head.isEquip = false;
                                _player.inventory.head = _player.inventory.items[nowPage * 5 + nowAction - 1];
                                _player.inventory.head.isEquip = true;
                                break;
                            case "상의":
                                _player.inventory.top.isEquip = false;
                                _player.inventory.top = _player.inventory.items[nowPage * 5 + nowAction - 1];
                                _player.inventory.top.isEquip = true;
                                break;
                            case "하의":
                                _player.inventory.bottom.isEquip = false;
                                _player.inventory.bottom = _player.inventory.items[nowPage * 5 + nowAction - 1];
                                _player.inventory.bottom.isEquip = true;
                                break;
                        }
                        SetStatus(ref _player);
                    }
                    else if (nowAction == 6)
                    {
                        nowPage--;
                        if (hasPrevPage) nowAction = 6;
                        else
                        {
                            if (hasNextPage) nowAction = 7;
                            else nowAction = 1;
                        }
                    }
                    else if (nowAction == 7)
                    {
                        nowPage++;
                        if (hasNextPage) nowAction = 7;
                        else
                        {
                            if (hasPrevPage) nowAction = 6;
                            else nowAction = 1;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        static void EnterShop(ref Player _player)
        {
            int nowAction = 0;
            string CheckAction(int number)
            {
                if (nowAction == number)
                {
                    return "<";
                }
                else
                {
                    return "";
                }
            }

            while (true)
            {
                Msg($"[뒤로가기] {CheckAction(0)}", 0, -2, true, true);

                Msg($"아이템 구매 {CheckAction(1)}", 0, 0, true, false);
                Msg($"아이템 판매 {CheckAction(2)}", 0, 1, true, false);

                MsgOutBox("방향키와 엔터를 이용해 행동을 선택하세요.");

                ConsoleKey key = Console.ReadKey().Key;
                if (key == ConsoleKey.UpArrow) nowAction = (int)MathF.Max(nowAction - 1, 0);
                else if (key == ConsoleKey.DownArrow) nowAction = (int)MathF.Min(nowAction + 1, 2);
                else if (key == ConsoleKey.Enter)
                {
                    switch (nowAction)
                    {
                        case 0: return;
                        case 1: EnterBuyItem(ref _player);break;
                        case 2: EnterSellItem(ref _player);break;
                    }
                }
            }
        }

        static void EnterBuyItem(ref Player _player)
        {
            int nowPage = 0;
            int nowAction = 0;
            string CheckAction(int number)
            {
                if (nowAction == number)
                {
                    return "<";
                }
                else
                {
                    return "";
                }
            }

            List<Item> shop = new List<Item>();

            switch (_player.classType)
            {
                case ClassType.Warrior:
                    shop.Add(new Item("본 크래셔", "무기", "맞으면 뼈도 못 추릴 망치", 500, 30, 5, 0, 0, 2, 0));
                    shop.Add(new Item("무쇠 대검", "무기", "무겁고 날카로운 대검", 1000, 50, 10, 0, 0, 4, 0));
                    shop.Add(new Item("드래곤 슬레이어", "무기", "용을 잡았다는 한손검", 5000, 90, 20, 0, 0, 7, 0));
                    shop.Add(new Item("엑스칼리버", "무기", "전설의 그 대검", 10000, 1000, 100, 0, 0, 10, 0));
                    break;
                case ClassType.Archer:
                    shop.Add(new Item("단궁", "무기", "최종병기 활", 500, 30, 10, 0, 200, 3, 0));
                    shop.Add(new Item("컴파운드 보우", "무기", "대한민국 양궁 화이팅", 1000, 50, 20, 0, 0, 5, 0));
                    shop.Add(new Item("세계수목 대궁", "무기", "세계수를 깎아만든 대궁", 5000, 100, 40, 0, 0, 8, 1));
                    shop.Add(new Item("아폴론의 활", "무기", "하프처럼 생겼다", 10000, 1000, 300, 0, 0, 12, 0));
                    break;
                case ClassType.Rogue:
                    shop.Add(new Item("비도", "무기", "던지는 용인데..", 500, 30, 0, 0, 0, 3, 0));
                    shop.Add(new Item("장미칼", "무기", "절삭력이 대단하다.", 1000, 60, 0, 0, 0, 5, 0));
                    shop.Add(new Item("단 검", "무기", "꿀처럼 달다.", 5000, 120, 10, 0, 0, 7, 0));
                    shop.Add(new Item("시그룬의 단검", "무기", "발키리 영웅의 파트너.", 10000, 1100, 150, 0, 200, 9, 2));
                    break;
                case ClassType.Mage:
                    shop.Add(new Item("나뭇가지", "무기", "누군가 마력을 넣어놨다.", 500, 50, 30, 0, 0, 4, 0));
                    shop.Add(new Item("효자손", "무기", "누군가 마력을 많이 넣어놨다.", 1000, 70, 50, 0, 0, 8, 0));
                    shop.Add(new Item("딱총나무 지팡이", "무기", "스투페파이.", 5000, 150, 100, 0, 0, 12, 0));
                    shop.Add(new Item("네크로노미콘", "무기", "악마의 책이 말을 건넨다. 시끄럽다.", 10000, 1100, 700, 0, 0, 16, 0));
                    break;
            }

            shop.Add(new Item("가죽 헬멧", "머리", "가죽으로 만든 헬멧.", 300, 0, 0, 30, 100, 2, 0));
            shop.Add(new Item("강철 헬름", "머리", "강철로 만든 헬멧.", 1000, 0, 0, 40, 300, 5, 0));
            shop.Add(new Item("퀴네에", "머리", "저승의 왕, 하데스의 투구. 투명화 기능은 없다.", 3000, 0, 0, 60, 500, 10, 0));

            shop.Add(new Item("꺼죽 상의", "상의", "동물꺼죽으로 만들었다.", 500, 0, 0, 20, 300, 0, 0));
            shop.Add(new Item("강철 갑옷", "상의", "웬만한 창으로도 뚫을 수 없다.", 2000, 0, 0, 30, 700, 0, 0));
            shop.Add(new Item("비테게의 갑옷", "상의", "전설의 대장장이 뷜란트의 역작.", 4500, 0, 0, 80, 1000, 0, 0));

            shop.Add(new Item("동물잠옷", "하의", "가죽으로 만든 바지. 만든이가 이름을 이상하게 붙였다.", 450, 0, 0, 30, 100, 0, 2));
            shop.Add(new Item("청바지", "하의", "청동으로 만든 각반.", 1500, 0, 0, 40, 300, 0, 5));
            shop.Add(new Item("발키리의 각반", "하의", "발키리들이 착용한 각반.", 3500, 0, 0, 100, 800, 0, 10));
            shop.Add(new Item("헤르메스의 각반", "하의", "날개가 달려있지만 날 수는 없다.", 3500, 0, 0, 50, 500, 0, 30));

            while (true)
            {
                bool hasPrevPage = false;
                bool hasNextPage = false;
                DrawBox();
                Console.SetCursorPosition(3, 2);
                Console.Write($"보유 골드: {_player.inventory.gold}");
                Msg($"[뒤로가기] {CheckAction(0)}", 0, -8, true, false);

                //상점 품목
                for (int i = 0; i < 5; i++)
                {
                    if (shop.Count > (nowPage * 5) + i)
                    {
                        Item nowItem = shop[(nowPage * 5) + i];
                        Msg($"{(nowPage * 5) + i + 1}. {nowItem.name} | {nowItem.type} | 공격력: {nowItem.damage} | 기량: {nowItem.damageRange} | 방어력: {nowItem.armor} | 체력: {nowItem.hp} | 금액: {nowItem.gold} {CheckAction(i + 1)}", 0, -6 + (i * 3), true, false);
                        Msg($"({nowItem.description})", 0, -6 + (i * 3) + 1, true, false);
                    }
                }

                if (nowPage > 0)
                {
                    hasPrevPage = true;
                    Msg($"[이전 페이지] {CheckAction(6)}", 0, 9, true, false);
                }
                if (shop.Count > (nowPage * 5) + 5)
                {
                    hasNextPage = true;
                    Msg($"[다음 페이지] {CheckAction(7)}", 0, 10, true, false);
                }
                MsgOutBox("구매할 아이템을 선택하세요.");

                ConsoleKey key = Console.ReadKey().Key;
                if (key == ConsoleKey.UpArrow)
                {
                    if (nowAction == 6)
                    {
                        int count = 5;
                        while (shop.Count < (nowPage * 5) + count)
                        {
                            count--;
                        }
                        nowAction = count;
                    }
                    else if (nowAction == 7)
                    {
                        if (hasPrevPage) nowAction = 6;
                        else
                        {
                            int count = 5;
                            while (shop.Count < (nowPage * 5) + count)
                            {
                                count--;
                            }
                            nowAction = count;
                        }
                    }
                    else nowAction = (int)MathF.Max(nowAction - 1, 0);
                }
                else if (key == ConsoleKey.DownArrow)
                {
                    //nowAction이 아이템을 가리킬 때
                    if (nowAction >= 1 && nowAction <= 5)
                    {
                        //다음 아이템이 있을 때
                        if (shop.Count > (nowPage * 5) + nowAction && nowAction < 5)
                        {
                            nowAction = (int)MathF.Min(nowAction + 1, 7);
                        }
                        //다음 아이템이 없을 때
                        else
                        {
                            if (hasPrevPage)
                            {
                                nowAction = 6;
                            }
                            else if (hasNextPage)
                            {
                                nowAction = 7;
                            }
                        }
                    }
                    //nowAction이 0, 6일 때
                    else
                    {
                        if (nowAction == 0)
                        {
                            if (shop.Count > (nowPage * 5) + nowAction && nowAction < 5)
                            {
                                nowAction = (int)MathF.Min(nowAction + 1, 7);
                            }
                            else
                            {
                                if (hasPrevPage)
                                {
                                    nowAction = 6;
                                }
                                else if (hasNextPage)
                                {
                                    nowAction = 7;
                                }
                            }
                        }
                        else if (nowAction == 6)
                        {
                            if (hasNextPage)
                            {
                                nowAction = 7;
                            }
                        }
                    }
                }
                else if (key == ConsoleKey.Enter)
                {
                    //구매
                    if (nowAction >= 1 && nowAction <= 5)
                    {
                        Item nowItem = shop[(nowPage * 5) + (nowAction - 1)];
                        if (_player.inventory.gold > nowItem.gold)
                        {
                            _player.inventory.gold -= nowItem.gold;
                            _player.inventory.items.Add(nowItem);
                            Msg("아이템을 구매하였습니다.", 0, 0, true, true);
                            Msg($"({nowItem.name})", 0, 1, false, false);
                        }
                        else
                        {
                            Msg("골드가 모자랍니다.", 0, 0, false, true);
                        }
                    }
                    else if (nowAction == 6)
                    {
                        nowPage--;
                        if (nowPage > 0) nowAction = 6;
                        else nowAction = 7;
                    }
                    else if (nowAction == 7)
                    {
                        nowPage++;
                        if (shop.Count > (nowPage * 5) + 5) nowAction = 7;
                        else nowAction = 6;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        static void EnterSellItem(ref Player _player)
        {
            int nowPage = 0;
            int nowAction = 0;
            string CheckAction(int number)
            {
                if (nowAction == number)
                {
                    return "<";
                }
                else
                {
                    return "";
                }
            }

            while (true)
            {
                bool hasPrevPage = false;
                bool hasNextPage = false;
                Msg($"[뒤로가기] {CheckAction(0)}", 0, -8, true, true);

                if (_player.inventory.items.Count < 1) nowAction = 0;

                //보유 아이템 5개 (페이지당)
                for (int i = 0; i < 5; i++)
                {
                    if (_player.inventory.items.Count > (nowPage * 5) + i)
                    {
                        Item nowItem = _player.inventory.items[(nowPage * 5) + i];
                        Msg($"{(nowPage * 5) + i + 1}. {nowItem.name} | {nowItem.type} | 공격력: {nowItem.damage} | 기량: {nowItem.damageRange} | 방어력: {nowItem.armor} | 체력: {nowItem.hp} | 금액: {nowItem.gold} {CheckAction(i + 1)}", 0, -6 + (i * 3), true, false);
                    }
                }

                if (nowPage > 0)
                {
                    hasPrevPage = true;
                    Msg($"[이전 페이지] {CheckAction(6)}", 0, 5, true, false);
                }
                if (_player.inventory.items.Count > (nowPage * 5) + 5)
                {
                    hasNextPage = true;
                    Msg($"[다음 페이지] {CheckAction(7)}", 0, 6, true, false);
                }
                MsgOutBox("방향키와 엔터를 이용해 행동을 선택하세요.");

                ConsoleKey key = Console.ReadKey().Key;
                if (key == ConsoleKey.UpArrow)
                {
                    if (nowAction == 6)
                    {
                        int count = 5;
                        while (_player.inventory.items.Count < (nowPage * 5) + count)
                        {
                            count--;
                        }
                        nowAction = count;
                    }
                    else if (nowAction == 7)
                    {
                        if (hasPrevPage) nowAction = 6;
                        else
                        {
                            int count = 5;
                            while (_player.inventory.items.Count < (nowPage * 5) + count)
                            {
                                count--;
                            }
                            nowAction = count;
                        }
                    }
                    else nowAction = (int)MathF.Max(nowAction - 1, 0);
                }
                else if (key == ConsoleKey.DownArrow)
                {
                    //nowAction이 아이템을 가리킬 때
                    if (nowAction >= 1 && nowAction <= 5)
                    {
                        //다음 아이템이 있을 때
                        if (_player.inventory.items.Count > (nowPage * 5) + nowAction && nowAction < 5)
                        {
                            nowAction = (int)MathF.Min(nowAction + 1, 7);
                        }
                        //다음 아이템이 없을 때
                        else
                        {
                            if (hasPrevPage)
                            {
                                nowAction = 6;
                            }
                            else if (hasNextPage)
                            {
                                nowAction = 7;
                            }
                        }
                    }
                    //nowAction이 0, 6일 때
                    else
                    {
                        if (nowAction == 0)
                        {
                            if (_player.inventory.items.Count > (nowPage * 5) + nowAction && nowAction < 5)
                            {
                                nowAction = (int)MathF.Min(nowAction + 1, 7);
                            }
                            else
                            {
                                if (hasPrevPage)
                                {
                                    nowAction = 6;
                                }
                                else if (hasNextPage)
                                {
                                    nowAction = 7;
                                }
                            }
                        }
                        else if (nowAction == 6)
                        {
                            if (hasNextPage)
                            {
                                nowAction = 7;
                            }
                        }
                    }
                }
                else if (key == ConsoleKey.Enter)
                {
                    if (nowAction >= 1 && nowAction <= 5)
                    {
                        Msg($"장착중이라면 장착이 해제됩니다.", 0, 1, true, true);
                        Msg($"{_player.inventory.items[nowPage * 5 + nowAction - 1].name} 아이템을", 0, 0, true, false);
                        Msg("판매하시겠습니까? (Y/N)", 0, 1, true, false);
                        MsgOutBox("Y = 네, N = 아니오");
                        string answer = Question("Y = 네, N = 아니오: ");
                        if (answer == "Y" || answer == "y")
                        {
                            Item nowItem = _player.inventory.items[nowPage * 5 + nowAction - 1];
                            if (nowItem.isEquip)
                            {
                                nowItem.isEquip = false;
                                if (_player.inventory.weapon == nowItem) _player.inventory.weapon = new Item();
                                else if (_player.inventory.head == nowItem) _player.inventory.head = new Item();
                                else if (_player.inventory.top == nowItem) _player.inventory.top = new Item();
                                else if (_player.inventory.bottom == nowItem) _player.inventory.bottom = new Item();
                            }
                            Msg("판매되었습니다.", 0, 0, false, true);
                            _player.inventory.gold += _player.inventory.items[nowPage * 5 + nowAction - 1].gold;
                            _player.inventory.items.Remove(_player.inventory.items[nowPage * 5 + nowAction - 1]);
                        }
                        else if (answer == "N" || answer == "n")
                        {
                            continue;
                        }
                        else
                        {
                            Msg("잘못된 값을 입력하셨습니다.", 0, 0, false, true);
                            continue;
                        }
                    }
                    else if (nowAction == 6)
                    {
                        nowPage--;
                        if (hasPrevPage) nowAction = 6;
                        else
                        {
                            if (hasNextPage) nowAction = 7;
                            else nowAction = 1;
                        }
                    }
                    else if (nowAction == 7)
                    {
                        nowPage++;
                        if (hasNextPage) nowAction = 7;
                        else
                        {
                            if (hasPrevPage) nowAction = 6;
                            else nowAction = 1;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        static void EnterDungeon(ref Player _player)
        {
            int nowAction = 0;
            string CheckAction(int number)
            {
                if (nowAction == number)
                {
                    return "<";
                }
                else
                {
                    return "";
                }
            }

            while (true)
            {
                Msg($"[뒤로가기] {CheckAction(0)}", 0, -2, true, true);

                Msg($"난이도: 쉬움 {CheckAction(1)}", 0, 0, true, false);
                Msg($"난이도: 보통 {CheckAction(2)}", 0, 1, true, false);
                Msg($"난이도: 어려움 {CheckAction(3)}", 0, 2, true, false);

                MsgOutBox("방향키와 엔터를 이용해 행동을 선택하세요.");

                ConsoleKey key = Console.ReadKey().Key;
                if (key == ConsoleKey.UpArrow) nowAction = (int)MathF.Max(nowAction - 1, 0);
                else if (key == ConsoleKey.DownArrow) nowAction = (int)MathF.Min(nowAction + 1, 3);
                else if (key == ConsoleKey.Enter)
                {
                    switch (nowAction)
                    {
                        case 0: return;
                        case 1: StartDungeon(ref _player, 0); break;
                        case 2: StartDungeon(ref _player, 1); break;
                        case 3: StartDungeon(ref _player, 2); break;
                    }
                }
            }
        }

        static void StartDungeon(ref Player _player, int _difficulty)
        {
            bool isEncounter = false;
            bool isTurn = true;

            int nowHP = _player.hp;
            int count = _difficulty + 1;
            Monster monster = new Monster();

            while (true)
            {
                DrawBox();

                //적을 모두 쓰러뜨리지 않았을 때
                if (count > 0)
                {

                    //적과 싸우는 중이 아닐때
                    if (!isEncounter)
                    {
                        isEncounter = true;
                        Random random = new Random();
                        int type = random.Next(0, 5);
                        switch (type)
                        {
                            case 0:
                                monster = new Monster("슬라임", _difficulty);
                                Msg($"슬라임이 나타났다!", 0, 0, false, true);
                                break;
                            case 1:
                                monster = new Monster("오크", _difficulty);
                                Msg($"오크가 나타났다!", 0, 0, false, true);
                                break;
                            case 2:
                                monster = new Monster("고블린", _difficulty);
                                Msg($"고블린이 나타났다!", 0, 0, false, true);
                                break;
                            case 3:
                                monster = new Monster("추방자", _difficulty);
                                Msg($"추방자가 나타났다!", 0, 0, false, true);
                                break;
                            case 4:
                                monster = new Monster("오우거", _difficulty);
                                Msg($"오우거가 나타났다!", 0, 0, false, true);
                                break;
                        }
                        DrawBox();
                        Console.SetCursorPosition(4, 2);
                        Console.Write($"{_player.name} HP: {nowHP}");

                        Console.SetCursorPosition(Screen_Width - 20, 2);
                        Console.Write($"{monster.name} HP: {monster.hp}");

                        Thread.Sleep((int)(1000 / GameSpeed));
                    }

                    //적과 싸우는 중일 때
                    else
                    {
                        if (isTurn)
                        {
                            Random random = new Random();
                            bool isHit = random.Next(1, 101) < ((1 - (monster.dodge / (_player.accuracy + _player.itemAccuracy))) * 100) ? true : false;
                            if (isHit)
                            {
                                int finalDamage = (int)MathF.Max(
                                    random.Next(
                                        (_player.damage + _player.itemDamage) - (_player.damageRange + _player.itemDamageRange), 
                                        (_player.damage + _player.itemDamage) + (_player.damageRange + _player.itemDamageRange) + 1)
                                    - monster.armor, 0);
                                monster.hp -= finalDamage;
                                Msg($"{monster.name}에게 {finalDamage}만큼의 피해를 가했다", 0, 0, true, false);

                                if (monster.hp <= 0)
                                {
                                    Thread.Sleep((int)(1000 / GameSpeed));
                                    Msg($"{monster.name}을 물리쳤다!", 0, 0, false, true);
                                    Msg($"{monster.exp} 경험치를 획득했다.", 0, 0, true, true);
                                    Msg($"({_player.exp}/{_player.maxExp}) => ({_player.exp + monster.exp}/{_player.maxExp})", 0, 1, false, false); ;
                                    _player.exp += monster.exp;
                                    
                                    if (_player.exp >= _player.maxExp)
                                    {
                                        _player.exp -= _player.maxExp;
                                        _player.maxExp = (int)(_player.maxExp * 1.2f);
                                        Msg($"레벨업! ({_player.level} => {++_player.level}).", 0, 0, false, true);
                                    }

                                    Msg($"{monster.gold} 골드를 획득했다.", 0, 0, false, true);
                                    Msg($"{_player.inventory.gold} => {_player.inventory.gold + monster.gold}", 0, 0, false, false);
                                    _player.inventory.gold += monster.gold;
                                    count--;
                                    isEncounter = false;
                                }
                            }
                            else
                            {
                                Msg($"공격이 빗나갔다!", 0, 0, true, false);
                            }
                            isTurn = false;
                        }
                        else
                        {
                            Random random = new Random();
                            bool isHit = random.Next(1, 101) < ((1 - ((_player.dodge + _player.itemDodge) / monster.accuracy)) * 100) ? true : false;
                            if (isHit)
                            {
                                int finalDamage = (int)MathF.Max(random.Next(monster.damage - monster.damageRange, monster.damage + monster.damageRange + 1) - (_player.armor + _player.itemArmor), 0);
                                nowHP -= finalDamage;
                                Msg($"{finalDamage}만큼의 피해를 입었다!", 0, 0, true, false);

                                if (nowHP <= 0)
                                {
                                    Thread.Sleep((int)(1000 / GameSpeed));
                                    Msg($"{monster.name}에게 털렸다..", 0, 0, false, true);
                                    Msg($"가진 골드의 절반을 잃어버렸다.", 0, 0, false, true);
                                    return;
                                }
                            }
                            else
                            {
                                Msg($"{monster.name}의 공격을 피했다!", 0, 0, true, false);
                            }
                            isTurn = true;
                        }

                        if (isEncounter)
                        {
                            Console.SetCursorPosition(4, 2);
                            Console.Write($"{_player.name} HP: {nowHP}");

                            Console.SetCursorPosition(Screen_Width - 20, 2);
                            Console.Write($"{monster.name} HP: {monster.hp}");

                            Thread.Sleep((int)(1000 / GameSpeed));
                        }
                        
                    }
                }
                else return;
            }
        }

        static void DrawBox()
        {
            Console.Clear();

            for (int i = 0; i < Screen_Width; i++)
            {
                Console.SetCursorPosition(i, 0);
                Console.Write("#");
                Console.SetCursorPosition(i, Screen_Height);
                Console.Write("#");
            }
            for (int i = 0; i < Screen_Height; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("#");
                Console.SetCursorPosition(Screen_Width, i);
                Console.Write("#");
            }
        }

        static void Msg(string content, int x = 0, int y = 0, bool isContinous = false, bool isNew = false)
        {
            if (isNew)
            {
                DrawBox();
            }
            int byteLength = System.Text.Encoding.Default.GetByteCount(content);
            int sizeOfContent = content.Length + ((byteLength - content.Length) / 2);
            Console.SetCursorPosition((Screen_Width / 2 - sizeOfContent / 2) + x, (Screen_Height / 2) + y);
            Console.Write(content);

            if (!isContinous)
            {
                Console.SetCursorPosition(0, Screen_Height + 1);
                Console.WriteLine("계속하려면 아무 키나 누르십시오.");
                Console.ReadKey();
            }
        }

        static void MsgOutBox(string content, int x = 0, int y = 0, bool isContinous = false)
        {
            Console.SetCursorPosition(0 + x, (Screen_Height + 1) + y);
            Console.WriteLine(content);
        }

        static string Question(string content)
        {
            Console.CursorVisible = true;
            Console.SetCursorPosition(0, Screen_Height + 1);
            Console.WriteLine("");
            Console.SetCursorPosition(0, Screen_Height + 1);
            Console.Write(content);
            string value = Console.ReadLine();
            Console.CursorVisible = false;
            return value;
        }

        static void SaveData(ref Player _player)
        {
            if (File.Exists("./data.json"))
            {
                string json = JsonConvert.SerializeObject(_player, Formatting.Indented);
                File.WriteAllText("./data.json", json);
            }
            else
            {
                string json = JsonConvert.SerializeObject(_player, Formatting.Indented);
                File.WriteAllText("./data.json", json);
            }
        }

        static void DeleteData(ref Player _player)
        {
            while (true)
            {
                Msg("데이터를 삭제하시겠습니까? (Y/N)", 0, 0, true, true);
                string answer = Question("입력: ");
                if (answer == "Y" || answer == "y")
                {
                    if (File.Exists("./Data.json"))
                    {
                        File.Delete("./Data.json");
                        _player = new Player();
                        break;
                    }
                }
                else if (answer == "N" || answer == "n")
                {
                    break;
                }
                else
                {
                    Msg("값을 잘못 입력하셨습니다.", 0, 0, false, true);
                }
            }
        }

        enum ClassType
        {
            None = 0,
            Warrior = 1,
            Archer = 2,
            Rogue = 3,
            Mage = 4
        }

        class Unit
        {
            public ClassType classType;
            public string className;
            public string classNameModifier;
            public string name = "";
            public int level;

            public int damage;
            public int damageRange;
            public int armor;
            public int hp;
            public int accuracy;
            public int dodge;
        }

        class Player : Unit
        {
            public int exp;
            public int maxExp;

            public Inventory inventory;
            public bool isCompleteTutorial;

            public int itemDamage;
            public int itemDamageRange;
            public int itemArmor;
            public int itemHp;
            public int itemAccuracy;
            public int itemDodge;

            public Player()
            {
                inventory = new Inventory();
                isCompleteTutorial = false;
            }
        }

        class Monster : Unit
        {
            public int gold;
            public int exp;

            public Monster()
            {

            }

            public Monster(string _name, int _difficulty)
            {
                Random random = new Random();

                name = _name;
                gold = random.Next(40, 120) + (_difficulty * random.Next(100, 200));
                exp = random.Next(30, 60) + (_difficulty * random.Next(50, 70));

                damage = random.Next(7, 15) + _difficulty * random.Next(30, 50);
                damageRange = random.Next(4, 7) + _difficulty * random.Next(30, 50);
                armor = random.Next(4, 8) + _difficulty * random.Next(30, 50);
                hp = random.Next(100, 130) + _difficulty * random.Next(30, 50);
                accuracy = random.Next(4, 7) + _difficulty * random.Next(2, 5);
                dodge = random.Next(0, 2) + _difficulty * random.Next(3, 5);
            }

        }

        class Inventory
        {
            public int gold = 0;

            public List<Item> items;
            public Item weapon;
            public Item head;
            public Item top;
            public Item bottom;

            public Inventory()
            {
                gold = 0;

                items = new List<Item>();
                weapon = new Item("없음", "", "");
                head = new Item("없음", "", "");
                top = new Item("없음", "", "");
                bottom = new Item("없음", "", "");
            }
        }

        static void GetItem(ref Player _player, Item item)
        {
            _player.inventory.items.Add(item);
        }

        class Item
        {
            public string name;
            public string description;
            public string type;
            public bool isEquip;
            public int gold;

            public int damage;
            public int damageRange;
            public int armor;
            public int hp;
            public int accuracy;
            public int dodge;

            public Item()
            {
                name = "없음";
                description = "";
                type = "";
                isEquip = false;
                gold = 0;

                damage = 0;
                damageRange = 0;
                armor = 0;
                hp = 0;
                accuracy = 0;
                dodge = 0;
            }

            public Item(string _name, string _type, string _description, int _gold = 0, int _damage = 0, int _damageRange = 0, int _armor = 0, int _hp = 0, int _accuracy = 0, int _dodge = 0)
            {
                name = _name;
                type = _type;
                description = _description;
                isEquip = false;
                gold = _gold;

                damage = _damage;
                damageRange = _damageRange;
                armor = _armor;
                hp = _hp;
                accuracy = _accuracy;
                dodge = _dodge;
            }
        }

    }
}