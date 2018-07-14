import BaseDataTypeConverter from "./BaseDataTypeConverter";

class TextDataTypeConverter extends BaseDataTypeConverter {
    toString(value) {
        return value;
    }

    fromString(str) {
        return str;
    }
}

export default TextDataTypeConverter;