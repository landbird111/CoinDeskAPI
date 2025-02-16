# 目前應用的技術有以下幾點:

## 架構部份

1. 考量需求的角度並不複雜，所以用了Minimal API的方式來開發

2. 框架使用了Clean Architecture的方式來開發

    - Model拆成三層
	    - ViewModel： 用來處理給UI的資料面
		- ServiceModel： 用來處理邏輯面
		- DataLayerModel： 用來處理資料面
	- 這樣的好處是可以讓每一層的職責更明確，也可以讓每一層的單元測試更容易
	- Minimal API負責處理UI的部份，Service負責處理邏輯的部份，DataLayer負責處理資料存取的部份

3. 用了Dependency Injection的方式來管理物件的生命週期
4. 資料庫使用了Entity Framework Core (LocalDB)
5. 多語系的部份，我使用了CultureInfo.CurrentCulture.Name來取得語系，並且實作了兩種寫法(皆實作在Service層)
	- 第一種是資料表
	    - 我開發了語系的資料表，將幣別和語系檔建成了主從式表格
		- 我覺得這樣的方式比較直覺，也比較容易維護(像是開發UI進行管理、比對、更新等等)
		- 在QueryCoindesk的API裡可以看到，Method名稱為ConvertCurrencyNameByDB
    - 第二種是資源檔
	    - 我實作了多語系的Resx檔，在QueryCoindesk的API裡可以看到，Method名稱為ConvertCurrencyNameByResx
6. 使用了Singleton的方式來建立快取
	- 因為查詢幣別的動作是簡單且重覆的，除了匯率會有變動外，幣別的資料應該是固定的，所以我將查詢幣別的結果建置快取
	- 但匯率是有時效性的，所以我會將匯率的資料快取起來，並且設定一個時間(目前設為30秒)，超過時間就重新查詢
	- 快取的建置，我使用MemoryCache
	- 實作在QueryCurrencyInfo的這支API裡

## Error Handling部份

1. UI層
    - 負責處理資料來源的檢驗，若有錯誤，則拋出過濾的錯誤訊息
	    - 建立一個自定義的ApiResponseViewModel，統一輸出錯誤訊息
2. Service層
	- 負責處理邏輯的部份，若有錯誤，則回應空值或空集合
		- 從UI來或是Service層來的查詢，皆可預期回傳結果
		- 此層皆有一個開發規範去加上try...catch，以確保回傳到預期的結果
3. Provider層
	- 負責處理資料存取的部份，這部份則不做例外處理
		- 此層考量經由Controller層過濾過的資料，已通過檢驗
		- Service層已經加上處理錯誤的部份，Provider層只需關注處理資料存取的作業即可

## 資料庫部份

1. 資料庫使用了Entity Framework Core (LocalDB)
2. 資料表考慮到多語系的需求，所以我將幣別和語系分開，成了主從式表格
3. 從Controller層提供了語系(langKey From CultureInfo.CurrentCulture.Name)的來源，也方便之後切換語系時，改變CultureInfo.CurrentCulture即可

## 測試部份
1. 單元測試的部份，尚未完成
    - 在許多的專案經驗下，常因為專案開發時程的壓力，單元測試常常被忽略
	- 使用了xUnit來做單元測試的經驗是極少的，所以尚未完成