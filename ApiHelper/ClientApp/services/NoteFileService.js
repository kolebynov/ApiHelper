import ApiException from "../exceptions/ApiException";

class NoteFileService {
    uploadFiles(noteId, files) {
        return new Promise((res, rej) => {
            const formData = new FormData();
            for (const file of files) {
                formData.append("files", file);
            }

            const xhr = new XMLHttpRequest();
            xhr.withCredentials = true;
            xhr.open("POST", `/api/notes/${noteId}/uploadFiles`);
            xhr.onreadystatechange = () => {
                if (xhr.readyState != 4) {
                    return;
                }

                const response = JSON.parse(xhr.responseText);
                if (response.success) {
                    res(response);
                }
                else {
                    rej(new ApiException(response.errors));
                }
            };
            xhr.send(formData);
        });
    }
}

export default new NoteFileService();