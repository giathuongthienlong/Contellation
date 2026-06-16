# DateTimePicker - Contellation Custom Controls

**DateTimePicker** là control chọn ngày giờ nâng cao, kết hợp giữa TextBox và Popup Calendar + Clock, hỗ trợ format linh hoạt và binding hai chiều.

---

## Tính năng nổi bật

- Hỗ trợ chọn ngày giờ đầy đủ (Date + Time)
- Popup CalendarWithClock đẹp và dễ sử dụng
- Format tùy chỉnh linh hoạt (`DateTimeFormat`)
- Binding hai chiều với `SelectedDateTime`
- Hỗ trợ Placeholder, Clear Button (qua TextBox nội bộ)
- Xử lý focus, keyboard và validation tốt
- Dễ tùy chỉnh style


## Dependency Properties

| Property          | Kiểu          | Mặc định                | Mô tả |
|-------------------|---------------|-------------------------|-------|
| `SelectedDateTime`| `DateTime?`   | `null`                  | Giá trị ngày giờ được chọn (TwoWay) |
| `DisplayDateTime` | `DateTime`    | `DateTime.Now`          | Ngày giờ hiển thị trên lịch |
| `DateTimeFormat`  | `string`      | `"dd/MM/yyyy HH:mm:ss"` | Định dạng hiển thị và parse |
| `IsDropDownOpen`  | `bool`        | `false`                 | Mở/đóng popup |
| `CalendarStyle`   | `Style`       | `null`                  | Style tùy chỉnh cho CalendarWithClock |
| `Text`            | `string`      | `""`                    | Nội dung TextBox |



## Cách sử dụng

### 1. Khai báo namespace

```xml
xmlns:ui="clr-namespace:Contellation.Custom.Controls;assembly=Contellation.Custom"
```

### 2. Ví dụ cơ bản

```xml

<ui:DateTimePicker 
    Width="280" Height="42"
    SelectedDateTime="{Binding SelectedDate, Mode=TwoWay}"
    DateTimeFormat="dd/MM/yyyy HH:mm:ss" />

<!-- Chỉ chọn ngày -->
<ui:DateTimePicker 
    DateTimeFormat="dd/MM/yyyy"
    SelectedDateTime="{Binding BirthDate}" />

```

### 3. Ví dụ nâng cao

```xml

<ui:DateTimePicker 
    DateTimeFormat="HH:mm dd/MM/yyyy"
    IsDropDownOpen="False"
    CalendarStyle="{StaticResource MyCustomCalendarStyle}"
    SelectedDateTimeChanged="DateTimePicker_SelectedDateTimeChanged" />

```

### Best Practices
Luôn set DateTimeFormat rõ ràng để tránh lỗi parse trên các máy có culture khác nhau.
Sử dụng binding TwoWay với SelectedDateTime.
Kết hợp với ResponsiveGrid để tạo form đẹp.
Nên đặt DateTimeFormat ở mức Application hoặc Window để đồng nhất.
Xử lý sự kiện SelectedDateTimeChanged để validate dữ liệu.