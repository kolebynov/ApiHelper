import BaseDataTypeConverter from "./BaseDataTypeConverter";

class NumberDataTypeConverter extends BaseDataTypeConverter {
    toString(value) {
        return value.toString();
    }

    fromString(str) {
        return parseFloat(str);
    }
}

export default NumberDataTypeConverter;