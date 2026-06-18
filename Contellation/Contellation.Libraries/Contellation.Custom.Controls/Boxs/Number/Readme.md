# NumberBox - Contellation Custom Controls

**NumberBox** là control nhập số nâng cao, hỗ trợ spin button, giới hạn giá trị, format số và các phím tắt tiện lợi.

---

## Tính năng nổi bật

- Hỗ trợ `Value` binding hai chiều (double?)
- Giới hạn `Minimum` / `Maximum`
- Spin button (+/-) bằng phím mũi tên và PageUp/PageDown
- Tùy chỉnh số chữ số thập phân (`MaxDecimalPlaces`)
- Clear Button tự động
- Hỗ trợ Icon và Placeholder (qua Attached Property)
- Sự kiện `EnterPressed`
- Format số đẹp theo culture (1,234,567.89)


## Dependency Properties

| Property            | Kiểu          | Mặc định          | Mô tả |
|---------------------|---------------|-------------------|-------|
| `Value`             | `bool`        | `null`            | Bật/tắt hiển thị grid lines để debug |
| `Minimum`           | `double`      | `double.MinValue` | Khoảng cách ngang giữa các cột |
| `Maximum`           | `double`      | `double.MaxValue` | Khoảng cách dọc giữa các hàng |
| `SmallChange`       | `double`      | `1.0`             | Cấu hình các breakpoint (XS, SM, MD, LG, XL) |
| `LargeChange`       | `double`      | `10.0`            | Cấu hình các breakpoint (XS, SM, MD, LG, XL) |
| `MaxDecimalPlaces`  | `int`         | `0`               | Cấu hình các breakpoint (XS, SM, MD, LG, XL) |
| `ClearButtonEnabled`| `bool`        | `true`            | Cấu hình các breakpoint (XS, SM, MD, LG, XL) |

## Sự kiện chính

| Event               | Mô tả |
|---------------------|-------|
| `EnterPressed`      | Kích hoạt khi nhấn Enter |
| `ValueChanged`      | Giá trị thay đổi (có thể mở rộng) |

## Phím tắt hỗ trợ

| Phím       | Mô Chức năng     |
|------------|------------------|
| `↑`        | Tăng SmallChange |
| `↓`        | Giảm SmallChange |
| `PageUp`   | Tăng LargeChange |
| `PageDown` | Giảm LargeChange |
| `Enter`    | Xác nhận giá trị |

---

## Cách sử dụng

### 1. Khai báo namespace

```xml
xmlns:ui="clr-namespace:Contellation.Custom.Controls;assembly=Contellation.Custom"
```

### 1. Ví dụ cơ bản

```xml
<ui:NumberBox 
    Value="{Binding Price, Mode=TwoWay}"
    MaxDecimalPlaces="2"
    DecimalPlaces="₫"
    Minimum="0"
    Maximum="1000000"
    SmallChange="100"
    LargeChange="1000"
    Width="280" Height="42" />
```

### 2. Ví dụ đầy đủ với Icon & Placeholder

```xml
<ui:NumberBox 
    Value="{Binding Amount, Mode=TwoWay}"
    ui:Element.PlaceholderText="Nhập số tiền..."
    ui:Element.Icon="&#x20AB;"   <!-- ₫ -->
    />
```

### Best Practices
1. Luôn set Minimum và Maximum khi có thể.
2. Sử dụng MaxDecimalPlaces phù hợp với loại số (0 cho tiền nguyên, 2 cho tiền tệ).
3. Kết hợp với ResponsiveGrid để tạo form đẹp.
4. Dùng EnterPressed event để xử lý logic xác nhận.
5. Nên dùng Attached Property el:Element.Icon và ui:Element.PlaceholderText để đồng bộ giao diện.